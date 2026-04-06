using HandwritenRecognition.Cross.Infrastructure.FileSystem;
using Microsoft.AspNetCore.Mvc;

namespace HandwritenRecognition.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class FilesController : ControllerBase
{
    [HttpGet("[action]/FileName={path:required}")]
    public async Task<IActionResult> DownloadFile(string path)
    {
        if (!System.IO.File.Exists(path))
            return NotFound();
        string contentType = FileUtil.GetContentType(path);
        return PhysicalFile(path, contentType);
    }
}