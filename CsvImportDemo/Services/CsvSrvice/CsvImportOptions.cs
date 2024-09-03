using System.Globalization;
using System.Text;

namespace CsvImportDemo.Services.CsvSrvice;

public class CsvImportOptions
{
    public Encoding FileEncoding { get; set; } = Encoding.UTF8;

    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

    public bool HasHeaderRecord { get; set; } = true;

    public char Delimiter { get; set; } = ',';
}
