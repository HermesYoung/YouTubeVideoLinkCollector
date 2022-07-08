using OfficeOpenXml;
using YoutubeVideoCollector.Interfaces;
using YoutubeVideoCollector.Models;

namespace YoutubeVideoCollector;

public class Exporter : IExporter, IDisposable
{
    private readonly ExcelPackage _excel;

    public Exporter(ExcelPackage excel)
    {
        _excel = excel;
    }

    public void Export(IEnumerable<VideoDetail>? videoDetails, string workSheetName)
    {
        var worksheet = _excel.Workbook.Worksheets.Add((workSheetName));

        if (videoDetails == null) return;
        foreach (var (detail, index) in videoDetails.Select(((detail, i) => (detail, i))))
        {
            worksheet.Cells[index + 1, 1].Value = detail.Title;
            worksheet.Cells[index + 1, 2].Value = "https://youtu.be/" + detail.VideoLink;
        }
    }

    public void Dispose()
    {
        _excel.Dispose();
    }
}