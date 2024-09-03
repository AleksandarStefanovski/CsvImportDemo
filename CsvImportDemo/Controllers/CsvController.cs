using CsvImportDemo.Models;
using CsvImportDemo.Services.CsvSrvice;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CsvDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CsvController : ControllerBase
{
    private readonly ICsvImportService<Person> _csvImportService;

    public CsvController(ICsvImportService<Person> csvImportService)
    {
        _csvImportService = csvImportService;
    }

    [HttpPost("import")]
    [RequestSizeLimit(200_000_000)]
    public async Task<IActionResult> ImportCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File not selected");

        var filePath = Path.GetTempFileName();

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var options = new CsvImportOptions
        {
            FileEncoding = System.Text.Encoding.UTF8,
            Culture = CultureInfo.InvariantCulture,
            HasHeaderRecord = true
        };

        _csvImportService.ImportFromCsv(filePath, options);

        return Ok("File imported successfully");
    }
}
