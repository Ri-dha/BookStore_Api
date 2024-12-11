using BookStoreTask.Utli;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTask.FilesMod;

[Route("files/")]
public class FilesController : BaseController
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }


    [HttpPost("upload-multiple")]
    [Consumes("multipart/form-data")] 
    [AllowAnonymous]
    public async Task<IActionResult> UploadMultipleFiles([FromForm] IFormFile[] files)
    {
        var savedFiles = await _fileService.SaveFilesAsync(files);
        return Ok(savedFiles);
    }
}