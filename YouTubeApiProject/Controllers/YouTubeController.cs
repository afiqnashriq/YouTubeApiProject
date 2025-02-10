using Microsoft.AspNetCore.Mvc;
using YouTubeApiProject.Models;
using YouTubeApiProject.Services;
using System.Threading.Tasks;
using System.Linq;

namespace YouTubeApiProject.Controllers
{
    public class YouTubeController : Controller
    {
        private readonly YouTubeApiService _youtubeService;
        private const int PageSize = 5; // Define PageSize

        public YouTubeController(YouTubeApiService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new YouTubeSearchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query, string duration = "any", string uploadDate = "any", int page = 1)
        {
            var allVideos = await _youtubeService.SearchVideosAsync(query, duration, uploadDate);
            int totalVideos = allVideos.Count(); // Fix Count issue
            int totalPages = (int)Math.Ceiling(totalVideos / (double)PageSize);

            var paginatedVideos = allVideos.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            var viewModel = new YouTubeSearchViewModel
            {
                Videos = paginatedVideos,
                CurrentPage = page,
                TotalPages = totalPages,
                Query = query,
                Duration = duration,
                UploadDate = uploadDate
            };

            return View("Index", viewModel);
        }
    }
}
