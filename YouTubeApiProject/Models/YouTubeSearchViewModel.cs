using System.Collections.Generic;

namespace YouTubeApiProject.Models
{
    public class YouTubeSearchViewModel
    {
        public List<YouTubeVideoModel> Videos { get; set; } = new(); // List of videos
        public int CurrentPage { get; set; } // The current page number
        public int TotalPages { get; set; } // The total number of pages
        public string Query { get; set; } = string.Empty; // The search query
    }
}
