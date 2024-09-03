using BenchmarkDotNet.Attributes;
using System.Net.Http.Headers;

namespace CsvImportBenchmarkDemo.Services;

// note: all csv files need to be included in the Data folder, if you want to test yours, and names changed of the csv file in this class 
public class CsvImportBenchmarkService
{
    private readonly HttpClient _client;
    private readonly string _apiBaseUrl;
    private const string fileName = "person_data.csv"; // change this into other csv file if you want to test other csv
    public CsvImportBenchmarkService()
    {
        _apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://csvimportdemo:8080";
        _client = new HttpClient
        {
            BaseAddress = new Uri(_apiBaseUrl)
        };
    }

    [Benchmark]
    public async Task BenchmarkCsvImport()
    {
        var filePath = $"/app/data/{fileName}";

        using var content = new MultipartFormDataContent();
        await using var fileStream = File.OpenRead(filePath);
        content.Add(new StreamContent(fileStream)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("text/csv") }
        }, "file", fileName);

        var response = await _client.PostAsync("/api/csv/import", content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Failed to import CSV: {response.ReasonPhrase}");
        }
    }
}
