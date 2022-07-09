using System.Text.Json;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using OfficeOpenXml;
using YoutubeVideoCollector;
using YoutubeVideoCollector.Exporter;
using YoutubeVideoCollector.Models;

ExcelExporter CreateExcelExporter()
{
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    var excel = new ExcelPackage();
    var excelExporter = new ExcelExporter(excel);
    return excelExporter;
}

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

var excelExporter = CreateExcelExporter();
var jsonExporter = new JsonExporter();
var manager = new ExporterManager();
manager.AddExporter("excel", excelExporter)
    .AddExporter("json", jsonExporter);

var videoGetter = new VideoGetter(youtubeService);
foreach (var channel in arguments.Channels)
{
    var videos = videoGetter.GetVideos(channel.ChannelId).ToList();
    foreach (var method in arguments.OutputMethods)
    {
        manager.BuildContent(method, videos, channel.OutputName);
    }
}

foreach (var method in arguments.OutputMethods)
{
    manager.Export(method, arguments.ExportFileName);
}



void GenerateAppsettings(string s)
{
    var exportArgument = new ExportArguments()
    {
        ApiKey = "Your Api Key",
        ExportFileName = "video.xlsx",
        Channels = Enumerable.Empty<TargetChannel>().Append(new TargetChannel()
        {
            ChannelId = "Target Channel Id",
            OutputName = "Sheet Name"
        }),
        OutputMethods = Enumerable.Empty<string>().Append("excel").Append("json")
    };
    var content = JsonSerializer.Serialize(exportArgument, new JsonSerializerOptions()
    {
        WriteIndented = true
    });
    using var streamWriter = new StreamWriter(s);
    streamWriter.WriteLine(content);
}