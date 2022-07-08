using YoutubeVideoCollector.Models;

namespace YoutubeVideoCollector.Interfaces;

public interface IExporter
{
    void Export(IEnumerable<VideoDetail>? videoDetails, string workSheetName);
}