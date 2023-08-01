using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace TDesign.Docs.ServerSide.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UploadController:ControllerBase
{
    [HttpPost("file")]
    public async Task<IActionResult> PostAsync([FromForm]IFormCollection form)
    {
        var savedPath = Path.Combine(Directory.GetCurrentDirectory(), "upload");

        if ( !Directory.Exists(savedPath) )
        {
            Directory.CreateDirectory(savedPath);
        }

        foreach ( var file in form.Files )
        {
            var generateFileName = string.Concat(DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff"), Path.GetExtension(file.FileName));
            var serverFilePath = Path.Combine(savedPath, generateFileName);

            using var fileStream = new FileStream(serverFilePath, FileMode.CreateNew);
            await file.OpenReadStream().CopyToAsync(fileStream);
        }

        return Ok();
    }
}
