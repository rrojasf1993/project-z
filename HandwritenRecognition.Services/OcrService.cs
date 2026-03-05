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

    public async Task UpdateJobResults()
    {
        var jobsToUpdate = unitOfWork.OcrJobs.Get(filter: x => x.Status == OcrJobStatus.Pending);
        foreach (var job in jobsToUpdate)
        {
            string url = "/api/processv2";
            InputFileDto inputFile = new InputFileDto() { Path = job.FileName };
            var result = await retryingHttpClient.PostAsync<InputFileDto,OcrResultDto?>(url, inputFile,null);
            if (result != null)
            {
                job.Status = OcrJobStatus.Completed;
                job.Error = null;
                
                var documentInstance = CreateDocument(result);
                await unitOfWork.OcrDocuments.AddAsync(documentInstance);
                await unitOfWork.SaveChangesAsync();
                
                var ocrResult = mapper.Map<OcrResult>(result);
                ocrResult.Lines=documentInstance.Lines.ToList();
                await unitOfWork.OcrResults.AddAsync(ocrResult);
                await unitOfWork.SaveChangesAsync();
                
                job.Result = ocrResult;
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                job.Status = OcrJobStatus.Failed;
            }
            
        }
    }

    private OcrDocument CreateDocument(OcrResultDto result)
    {
        OcrDocumentDto? doc = mapper.Map<OcrDocumentDto>(result);
        var documentEntity=mapper.Map<OcrDocument>(doc);
        return documentEntity;
    }
}