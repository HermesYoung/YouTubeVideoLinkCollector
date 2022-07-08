namespace YoutubeVideoCollector.Models;

public class ExportArguments
{
    public string ApiKey { get; set; }
    public string WorkBookName { get; set; }
    public IEnumerable<TargetChannel> Channels { get; set; }
}