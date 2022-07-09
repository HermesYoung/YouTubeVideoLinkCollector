using Google.Apis.YouTube.v3.Data;

namespace YoutubeVideoCollector.Interfaces;

public interface IExporter : IDisposable
{
    void BuildContent(IEnumerable<PlaylistItem> videoDetails, string exportName);
    void Export(string filename);
}