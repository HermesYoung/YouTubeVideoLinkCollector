namespace YoutubeVideoCollector.Models;

public class ExportDetail
{
    public string Name { get; set; }
    public IEnumerable<VideoDetail> Videos { get; set; }
}