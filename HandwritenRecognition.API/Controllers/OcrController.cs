using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Infrastructure.FileSystem;
using HandwritenRecognition.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HandwritenRecognition.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class OcrController(IOcrService ocrService, ILogger<OcrController> logger) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<ActionResult<List<OcrJobDto>>> ProcessOcrRequest([FromForm]IFormFile[] files)
    {
        try
        {
            var tempFiles=Request.Form.Files;
            List<OcrJobDto> ocrJobs = new List<OcrJobDto>();
            foreach (var file in tempFiles)
            {
                var result = await ocrService.CreateOcrJob(file);
                if(result is not null)
                    ocrJobs.Add(result);
            } 
            return Ok(ocrJobs);
        }
        catch (Exception e)
        {
            logger.LogError("{EMessage}\n\n{EStackTrace}", e.Message, e.StackTrace);
            return StatusCode(500);
        }
    }

    [HttpGet("[action]/StatusId={statusId:int}")]
    [HttpGet("[action]/statusId={statusId:int}&StartDate={startDate:datetime}&EndDate={endDate:datetime}")]
    public ActionResult<List<OcrJobDto>> GetOcrJobsByStatus(int statusId, DateTime? startDate=null, DateTime? endDate=null)
    {
        List<OcrJobDto> ocrJobs;
        try
        {
            ocrJobs=ocrService.GetOcrJobsByStatus((OcrJobStatus)statusId,startDate,endDate);
            return Ok(ocrJobs);
        }
        catch (Exception e)
        {
            logger.LogError("{EMessage}\n\n{EStackTrace}", e.Message, e.StackTrace);
            return StatusCode(500);
        }
    }
    
    [HttpGet("[action]/jobId={jobId:guid}")]
    public async Task<ActionResult> DownloadDocumentImageForJob(Guid jobId)
    {
        try
        {
            var jobInfo=await ocrService.GetOcrJobById(jobId);
            if (jobInfo is null)
                return NotFound("Job not found");
            if (!System.IO.File.Exists(jobInfo.PreprocessedFileName))
                return NotFound();
            string contentType = FileUtil.GetContentType(jobInfo.PreprocessedFileName);
            return PhysicalFile(jobInfo.PreprocessedFileName, contentType);
        }
        catch (Exception e)
        {
            logger.LogError("{EMessage}\n\n{EStackTrace}", e.Message, e.StackTrace);
            return StatusCode(500);
        }
        
    }
    
   
}