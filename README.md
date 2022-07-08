# YouTubeVideoCollector
Collect video links from youtube channel.

# How to Build

## Requirement
- .Net 6.0 Sdk

## Build
1. Clone or download this repository.
2. Run following script.
```sh
dotnet build -c Release -o <output path>
```

# How to Use

## Add json file

appsettings.json
```json

{
  "ApiKey": "Your Api Key",
  "WorkBookName": "video.xlsx",
  "Channels": [
    {
      "ChannelId": "Target Channel Id",
      "OutputSheetName": "Sheet Name"
    }
  ]
}

```
How to get ApiKey https://cloud.google.com/docs/authentication/api-keys

## Run
Run program with appsettings.json in the same directory
```sh
YouTubeVideoCollector
```
or

example 
```sh
YouTubeVideoCollector ./path/to/file.json
```
