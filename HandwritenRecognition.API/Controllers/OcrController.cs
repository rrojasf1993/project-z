using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HandwritenRecognition.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class OcrController : ControllerBase
{
    private readonly IOcrService _ocrService;
    private readonly ILogger<OcrController> _logger;

    public OcrController(IOcrService ocrService, ILogger<OcrController> logger)
    {
        _ocrService = ocrService;
        _logger = logger;
    }

    
    [HttpPost("[action]")]
    public async Task<ActionResult<List<OcrJobDto>>> ProcessOcrRequest([FromForm]IFormFile[] files)
    {
        try
        {
            var tempFiles=Request.Form.Files;
            List<OcrJobDto> ocrJobs = new List<OcrJobDto>();
            foreach (var file in tempFiles)
            {
                var result = await _ocrService.CreateOcrJob(file);
                if(result is not null)
                    ocrJobs.Add(result);
            } 
            return Ok(ocrJobs);
        }
        catch (Exception e)
        {
            _logger.LogError("{EMessage}\n\n{EStackTrace}", e.Message, e.StackTrace);
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
            ocrJobs=_ocrService.GetOcrJobsByStatus((OcrJobStatus)statusId);
            return Ok(ocrJobs);
        }
        catch (Exception e)
        {
            _logger.LogError("{EMessage}\n\n{EStackTrace}", e.Message, e.StackTrace);
            return StatusCode(500);
        }
    }
    
   
}