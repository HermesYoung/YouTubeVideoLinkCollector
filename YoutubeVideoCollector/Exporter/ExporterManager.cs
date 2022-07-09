using Google.Apis.YouTube.v3.Data;
using YoutubeVideoCollector.Interfaces;

namespace YoutubeVideoCollector.Exporter;

public class ExporterManager
{
    private readonly Dictionary<string, IExporter> _exporters = new();

    public ExporterManager AddExporter(string key ,IExporter exporter)
    {
        _exporters.Add(key.ToLower(), exporter);
        return this;
    }

    public void BuildContent(string key, IEnumerable<PlaylistItem> playlistItems, string exportName)
    {
        _exporters[key].BuildContent(playlistItems, exportName);
    }

    public void Export(string key, string filename)
    {
        _exporters[key].Export(filename);
        _exporters[key].Dispose();
    }
}