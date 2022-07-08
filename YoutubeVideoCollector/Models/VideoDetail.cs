namespace YoutubeVideoCollector.Models;

public class VideoDetail
{
    public string VideoLink { get; set; }
    public string Title { get; set; }

    public VideoDetail(string videoId, string title)
    {
        VideoLink = "https://youtu.be/" + videoId;
        Title = title;
    }
}