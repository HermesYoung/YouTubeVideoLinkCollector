using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YoutubeVideoCollector;

public class VideoGetter
{
    private readonly YouTubeService _service;
    public VideoGetter(YouTubeService service)
    {
        _service = service;
    }
    public  IEnumerable<PlaylistItem> GetVideos(string chanel)
    {
        var channelRequest = _service.Channels.List("contentDetails");
        channelRequest.Id = chanel;

        var result = channelRequest.Execute();
        var uploads = result.Items.First().ContentDetails.RelatedPlaylists.Uploads;

        var videoDetails = Enumerable.Empty<PlaylistItem>();
        var next = string.Empty;
        while (next != null)
        {
            var playListRequest = _service.PlaylistItems.List("contentDetails,snippet");
            playListRequest.PlaylistId = uploads;
            playListRequest.MaxResults = 50;
            playListRequest.PageToken = next;
            var playlistItemListResponse = playListRequest.Execute();
            var videos = playlistItemListResponse.Items.ToList();
            next = playlistItemListResponse.NextPageToken;
            videoDetails = videoDetails.Concat(videos);
        }
        return videoDetails;
    }
}