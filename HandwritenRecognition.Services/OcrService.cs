using System.Linq.Expressions;
using AutoMapper;
using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Infrastructure;
using HandwritenRecognition.Cross.Infrastructure.FileSystem;
using HandwritenRecognition.Data.Entities;
using HandwritenRecognition.Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HandwritenRecognition.Services;

public class OcrService(
    IRetryingHttpClient retryingHttpClient,
    IUnitOfWork unitOfWork,
    ILogger<OcrService> logger,
    IMapper mapper)
    : IOcrService
{
    private readonly ILogger<OcrService> _logger = logger;


    public async Task<OcrJobDto?> CreateOcrJob(IFormFile file)
    {
        var jobId = Guid.NewGuid();
        var newJob = new OcrJobDto
        {
            JobId = jobId,
            Status = OcrJobStatus.Pending,
        };
        await using (var tempStream = file.OpenReadStream())
        {
            var tempFilePath = await FileUtil.SaveUploadedFile(tempStream, file.FileName, "uploadedDocuments");
            newJob.FileName = tempFilePath;
        }

        OcrJobStore.Jobs[jobId] = newJob;
        await CreateDraftJob(newJob);
        return newJob;
    }

    public Task<OcrJobDto?> CreateBatchOcrJob(IFormFileCollection files)
    {
        throw new NotImplementedException("Aun no estamos listos para esta conversacion");
    }


    private async Task CreateDraftJob(OcrJobDto jobData)
    {
        OcrJob? ocrJobToInsert = mapper.Map<OcrJob>(jobData);
        await unitOfWork.OcrJobs.AddAsync(ocrJobToInsert);
        await unitOfWork.SaveChangesAsync();
    }

    public IEnumerable<OcrJobDto> GetPendingJobsFromDatabase()
    { 
        var jobsToUpdate = unitOfWork.OcrJobs.Get(filter: x => x.Status == OcrJobStatus.Pending);
        return mapper.Map<List<OcrJobDto>>(jobsToUpdate);
    }

    public async Task<OcrResultDto?> UpdateJobResultsByJobId(Guid jobId)
    {
            var jobToUpdate = await unitOfWork.OcrJobs.GetByIdAsync(jobId);
            if (jobToUpdate is null)
                throw new ArgumentNullException(nameof(jobToUpdate));
            string url = "/api/processv2";
            InputFileDto inputFile = new InputFileDto() { Path = jobToUpdate.FileName };
            try
            {
                var result = await retryingHttpClient.PostAsync<InputFileDto,OcrResultDto?>(url, inputFile,null);
                if (result != null)
                {
                    ConfigureFrontendFriendlyBoxes(result.Lines);
                    var documentInstance=await CreateDocument(result);
                    await CreateResultRecord(result, jobToUpdate,documentInstance);
                    jobToUpdate.Status = OcrJobStatus.Completed;
                    jobToUpdate.Error = null;
                    jobToUpdate.PreprocessedFileName = result.PreProcessImgPath;
                }
                else
                {
                    jobToUpdate.Status = OcrJobStatus.Failed;
                }
                
                jobToUpdate.UpdatedAt=DateTime.Now;
                await unitOfWork.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {
                jobToUpdate.Status = OcrJobStatus.Failed;
                jobToUpdate.Error = e.Message+"\n"+e.StackTrace;
                await unitOfWork.SaveChangesAsync();
                return default;
            }
    }

    private async Task CreateResultRecord(OcrResultDto? result, OcrJob job, OcrDocument document)
    {
        var ocrResult = mapper.Map<OcrResult>(result); 
        job.Result=ocrResult;
        ocrResult.Lines = document.Lines.ToList();
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<OcrJobDto?> GetOcrJobById(Guid id)
    {
        try
        {
            var job = await unitOfWork.OcrJobs.GetByIdAsync(id);
            var result = mapper.Map<OcrJobDto>(job);
            return result; 
        }
        catch (Exception e)
        {
            _logger.LogError("An error has ocurred querying jobs with id ... {0}", id);
            _logger.LogError($"{e.Message}\n{e.StackTrace}");
            return null;
        }
    }

    public List<OcrJobDto> GetOcrJobsByStatus(OcrJobStatus status, DateTime? startDate=null, DateTime? endDate=null)
    {
        List<OcrJobDto> jobs = new List<OcrJobDto>();
        Expression<Func<OcrJob,bool>> filterExpresion = o => o.Status == status;
        if(startDate is not null && endDate is not null)
            filterExpresion=o=>o.Status==status && o.UpdatedAt>=startDate && o.UpdatedAt<=endDate;
        try
        {   _logger.LogInformation("Getting jobs with status ... {0}", status);
            var query= unitOfWork.OcrJobs.Get(filter:filterExpresion, includeProperties:"Result");
            jobs = mapper.Map<List<OcrJobDto>>(query);
        }
        catch (Exception e)
        {
            logger.LogError(
                $"An error has ocurred querying jobs with status {status} ...\\n{e.Message}\\{e.StackTrace}");
        }

        return jobs;
    }

    public OcrJobDto GetOcrJobByDocumentId(Guid id)
    {
        _logger.LogInformation("Getting document with id ... {0}", id);
        OcrJobDto? documentInfo = null;
        try
        {
            var document =  unitOfWork.OcrDocuments.Get(filter:doc=>doc.Id == id && doc.Status==OcrDocumentStatus_.PendingReview, includeProperties:"Lines").FirstOrDefault();
            if (document is not null)
            {
                var linesIds = document.Lines.Select(l => l.Id);
                var result =unitOfWork.OcrResults.Get(filter:l=>l.Lines.Select(s=>s.Id).All(lrs=>linesIds.Contains(lrs)), includeProperties:"Lines").FirstOrDefault();
                var job = unitOfWork.OcrJobs.Get(j=>j.Result==result, includeProperties:"Result").FirstOrDefault();
                documentInfo = mapper.Map<OcrJobDto>(job);
            }
        }
        catch (Exception e)
        {
            logger.LogError(
                $"An error has ocurred querying the job for document with id {id} ...\\n{e.Message}\\{e.StackTrace}");
        }
        return documentInfo;
    }

    private async Task<OcrDocument> CreateDocument(OcrResultDto result)
    {
        OcrDocumentDto? doc = mapper.Map<OcrDocumentDto>(result);
        var documentEntity=mapper.Map<OcrDocument>(doc);
        await unitOfWork.OcrDocuments.AddAsync(documentEntity);
        await unitOfWork.SaveChangesAsync();
        return documentEntity;
    }
    
    private void ConfigureFrontendFriendlyBoxes(List<OcrLineDto> lines)
    {
        foreach (var line in lines)
        {
            line.FrontendFriendlyBoxes = new List<OcrBoundingBoxDto>(){Util.ConvertFromPaddle(line.BoundingBox)};
        }        
    }
}