using Microsoft.AspNetCore.Mvc;
using YouTubeApiProject.Services;
using YouTubeApiProject.Models;
using System.Threading.Tasks;

namespace YouTubeApiProject.Controllers
{
    public class YouTubeController : Controller
    {
        private readonly YouTubeApiService _youtubeService;
        private const int PageSize = 5; // Number of videos per page

        public YouTubeController(YouTubeApiService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        // Display Search Page
        public IActionResult Index()
        {
            return View(new YouTubeSearchViewModel()); // Pass an empty ViewModel initially
        }

        // Handle search query with pagination
        [HttpPost]
        public async Task<IActionResult> Search(string query, int page = 1)
        {
            var allVideos = await _youtubeService.SearchVideosAsync(query);
            int totalVideos = allVideos.Count;
            int totalPages = (int)Math.Ceiling(totalVideos / (double)PageSize);

            var paginatedVideos = allVideos.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            var viewModel = new YouTubeSearchViewModel
            {
                Videos = paginatedVideos,
                CurrentPage = page,
                TotalPages = totalPages,
                Query = query
            };

            return View("Index", viewModel);
        }
    }
}
