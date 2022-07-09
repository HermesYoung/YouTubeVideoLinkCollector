using System.Text.Encodings.Web;
using System.Text.Json;
using Google.Apis.YouTube.v3.Data;
using YoutubeVideoCollector.Interfaces;
using YoutubeVideoCollector.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace YoutubeVideoCollector.Exporter;

public class JsonExporter : IExporter
{
    private IEnumerable<ExportDetail> _exportDetails = Enumerable.Empty<ExportDetail>();
    public void BuildContent(IEnumerable<PlaylistItem> videoDetails, string exportName)
    {
        var videos = videoDetails.Select(x =>
        {
            if (x.Snippet.PublishedAt != null)
                return new VideoDetail()
                {
                    Title = x.Snippet.Title,
                    Link = "https://youtu.be/" + x.ContentDetails.VideoId,
                    PublishAt = x.Snippet.PublishedAt.Value
                };
            return new VideoDetail()
            {
                Title = x.Snippet.Title,
                Link = "https://youtu.be/" + x.ContentDetails.VideoId
            };
        });

        _exportDetails = _exportDetails.Append(new ExportDetail()
        {
            Name = exportName,
            Videos = videos
        });
    }

    public void Export(string filename)
    {
        using var streamWriter = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), filename + ".json"));
        var json = JsonSerializer.Serialize(_exportDetails, new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        streamWriter.WriteLine(json);
    }

    public void Dispose()
    {
    }
}