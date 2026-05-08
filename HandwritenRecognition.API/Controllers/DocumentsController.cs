using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Infrastructure.FileSystem;
using HandwritenRecognition.Services;
using Microsoft.AspNetCore.Mvc;

namespace HandwritenRecognition.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DocumentsController(
    IDocumentService documentService,
    ILogger<DocumentsController> logger,
    IOcrService ocrService)
    : ControllerBase
{
    private readonly ILogger<DocumentsController> _logger = logger;
    private readonly IDocumentService _documentService = documentService;
    private readonly IOcrService _ocrService = ocrService;

    [HttpGet("[action]/StatusId={statusId:int}")]
    [HttpGet("[action]/StatusId={statusId:int}&StartDate={startDate:datetime}&EndDate={endDate:datetime}")]

    public ActionResult<List<OcrDocumentDto>> GetDocumentsByStatus(int statusId, DateTime? startDate = null,
        DateTime? endDate = null)
    {
        try
        {
            _logger.LogInformation($"Getting documents with  {statusId} ...");
            var result = _documentService.GetOcrDocumentsByStatus((OcrDocumentStatus_)statusId, startDate, endDate);
            if (result is null || result.Count == 0)
                return StatusCode(StatusCodes.Status204NoContent);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError($"An error has ocurred querying the documents with status {statusId}",
                $"{e.Message}\n{e.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("[action]/Document={documentId:guid}")]
    public ActionResult<List<OcrDocumentDto>> GetDocumentDetailsById(Guid documentId)
    {
        try
        {
            _logger.LogInformation($"Getting job data for document with  {documentId} ...");
            var result = _ocrService.GetOcrJobByDocumentId(documentId);
            if (result is null)
                return StatusCode(StatusCodes.Status204NoContent);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError($"An error has ocurred querying the documents with id {documentId}",
                $"{e.Message}\n{e.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPatch("[action]")]
    public async Task< ActionResult> UpdateDocumentLines(OcrDocumentDto modifiedDocument)
    {
        try
        {
            var result=await _documentService.UpdateOcrDocument_WithHumanCorrections(modifiedDocument);
            if (result is null)
                return StatusCode(StatusCodes.Status204NoContent);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError($"An error has ocurred updating the human made corrections for document with id {modifiedDocument.Id} \n{e.Message} \n{e.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
}