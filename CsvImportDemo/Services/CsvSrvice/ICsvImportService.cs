namespace CsvImportDemo.Services.CsvSrvice;

public interface ICsvImportService<T>
{
    void ImportFromCsv(string filePath, CsvImportOptions options);
}
