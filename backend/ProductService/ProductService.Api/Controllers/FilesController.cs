using Microsoft.AspNetCore.Mvc;

namespace ProductService.Api.Controllers;


[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSizeInBytes = 2 * 1024 * 1024;

    private readonly IWebHostEnvironment environment;

    public FilesController(IWebHostEnvironment environment)
    {
        this.environment = environment;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Debe seleccionar un archivo.");

        if (file.Length > MaxFileSizeInBytes)
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La imagen no puede superar los 2 MB.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Formato no permitido. Use jpg, jpeg, png o webp.");

        var webRootPath = environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot");
        var uploadsPath = Path.Combine(webRootPath, "uploads");

        Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsPath, fileName);

        await using var stream = System.IO.File.Create(filePath);
        await file.CopyToAsync(stream, cancellationToken);

        var url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

        return Ok(new { url });
    }
}