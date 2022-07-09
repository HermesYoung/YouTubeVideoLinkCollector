namespace YoutubeVideoCollector.Models;

public class ExportArguments
{
    public string ApiKey { get; set; }
    public string ExportFileName { get; set; }
    public IEnumerable<TargetChannel> Channels { get; set; }
    public IEnumerable<string> OutputMethods { get; set; }
}

