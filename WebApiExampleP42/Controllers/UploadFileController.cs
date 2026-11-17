using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;
using WebApiExampleP42.Services;


namespace WebApiExampleP42.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadFileController(FileUploadService fileUploadService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {

        var uniqueFileName = await fileUploadService.UploadFileAsync(file);

        return Ok(new { message = "File uploaded successfully.", fileName = uniqueFileName });
    }




}
