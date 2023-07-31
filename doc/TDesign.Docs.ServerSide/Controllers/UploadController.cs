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
        var savedPath = "/upload";

        if ( !Directory.Exists(savedPath) )
        {
            Directory.CreateDirectory(savedPath);
        }
        foreach ( var file in files )
        {
            var generateFileName = Path.Combine(DateTimeOffset.UnixEpoch.ToString(), Path.GetExtension(file.FileName));
            var serverFilePath = Path.Combine(savedPath, generateFileName);

            using var fileStream = new FileStream(serverFilePath, FileMode.CreateNew);
            await file.OpenReadStream().CopyToAsync(fileStream);
        }

        return Ok();
    }
}
