using System.Text.Json;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using OfficeOpenXml;
using YoutubeVideoCollector;
using YoutubeVideoCollector.Models;

var path = string.IsNullOrEmpty(args.FirstOrDefault())? Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json") : args.First();

string json;
try
{
    using var streamReader = new StreamReader(path);
    json = streamReader.ReadToEnd();
}
catch (FileNotFoundException e)
{
    Console.Error.WriteLine(e.Message);
    Console.WriteLine($"Generate {path}");
    GenerateAppsettings(path);
    return;
}

var arguments = JsonSerializer.Deserialize<ExportArguments>(json);
if (arguments == null)
{
    Console.Error.WriteLine($"Can not serialize {path}.");
    return;
}
var youtubeService = new YouTubeService(new BaseClientService.Initializer()
{
    ApiKey = arguments.ApiKey,
});
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
using var excel = new ExcelPackage();
var exporter = new Exporter(excel);
var videoGetter = new VideoGetter(youtubeService);
foreach (var channel in arguments.Channels)
{
    var videos = videoGetter.GetVideos(channel.ChannelId);
    exporter.Export(videos, channel.OutputSheetName);
}

excel.SaveAs(arguments.WorkBookName);

void GenerateAppsettings(string s)
{
    var exportArgument = new ExportArguments()
    {
        ApiKey = "Your Api Key",
        WorkBookName = "video.xlsx",
        Channels = Enumerable.Empty<TargetChannel>().Append(new TargetChannel()
        {
            ChannelId = "Target Channel Id",
            OutputSheetName = "Sheet Name"
        })
    };
    var content = JsonSerializer.Serialize(exportArgument, new JsonSerializerOptions()
    {
        WriteIndented = true
    });
    using var streamWriter = new StreamWriter(s);
    streamWriter.WriteLine(content);
}