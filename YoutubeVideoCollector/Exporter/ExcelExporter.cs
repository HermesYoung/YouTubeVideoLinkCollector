using Google.Apis.YouTube.v3.Data;
using OfficeOpenXml;
using YoutubeVideoCollector.Interfaces;

namespace YoutubeVideoCollector.Exporter;

public class ExcelExporter : IExporter, IDisposable
{
    private readonly ExcelPackage _excel;

    public ExcelExporter(ExcelPackage excel)
    {
        _excel = excel;
    }

    public void BuildContent(IEnumerable<PlaylistItem> videoDetails, string exportName)
    {
        var worksheet = _excel.Workbook.Worksheets.Add((exportName));

        if (videoDetails == null) return;
        worksheet.Cells[1, 1].Value = "Publish Time";
        worksheet.Cells[1, 2].Value = "Title";
        worksheet.Cells[1, 3].Value = "Link";
        foreach (var (detail, index) in videoDetails.Select(((detail, i) => (detail, i))))
        {
            worksheet.Cells[index + 2, 1].Value = detail.Snippet.PublishedAt?.ToString("yyyy/MM/dd");
            worksheet.Cells[index + 2, 2].Value = detail.Snippet.Title;
            worksheet.Cells[index + 2, 3].Value = "https://youtu.be/" + detail.ContentDetails.VideoId;
        }
    }

    public void Export(string filename)
    {
        _excel.SaveAs(filename + ".xlsx");
    }

    public void Dispose()
    {
        _excel.Dispose();
    }
}