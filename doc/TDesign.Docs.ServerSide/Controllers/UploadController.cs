using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace TDesign.Docs.ServerSide.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UploadController:ControllerBase
{
    [HttpPost("file")]
    public async Task<IActionResult> PostAsync([FromForm]List<IFormFile> files)
    {
        //var savedPath = "/upload";
        //var generateFileName = Path.Combine(DateTimeOffset.UnixEpoch.ToString(), Path.GetExtension(file.Name));

        //if ( !Directory.Exists(savedPath) )
        //{ 
        //    Directory.CreateDirectory(savedPath);
        //}

        //var serverFilePath = Path.Combine(savedPath, generateFileName);

        //using var fileStream = new FileStream(serverFilePath, FileMode.CreateNew);
        //await file.OpenReadStream().CopyToAsync(fileStream);

        return Ok();
    }
}
