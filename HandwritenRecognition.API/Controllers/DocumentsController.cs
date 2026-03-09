using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Services;
using Microsoft.AspNetCore.Mvc;

namespace HandwritenRecognition.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DocumentsController(IDocumentService documentService, ILogger<DocumentsController> logger)
    : ControllerBase
{
    private readonly ILogger<DocumentsController> _logger = logger;
    private readonly IDocumentService _documentService = documentService;

    [HttpGet("[action]/{statusId}")]
    public ActionResult<List<OcrDocumentDto>> GetDocumentsByStatus(int statusId, int pageSize)
    {
        try
        {
            _logger.LogInformation($"Getting documents with  {statusId} ...");
            var result=_documentService.GetOcrDocumentsByStatus((OcrDocumentStatus_)statusId);
            if(result is null || result.Count == 0)
                return StatusCode(StatusCodes.Status204NoContent);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError($"An error has ocurred querying the documents with status {statusId}", $"{e.Message}\n{e.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}