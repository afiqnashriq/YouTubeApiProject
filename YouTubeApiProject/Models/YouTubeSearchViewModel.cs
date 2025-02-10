namespace YouTubeApiProject.Models
{
    public class YouTubeSearchViewModel
    {
        public string Query { get; set; } = string.Empty;
        public string Duration { get; set; } = "any";
        public string UploadDate { get; set; } = "any";
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<YouTubeVideoModel> Videos { get; set; } = new();
    }
}
