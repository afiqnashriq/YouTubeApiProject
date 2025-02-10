using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTubeApiProject.Services
{
    public class YouTubeApiService
    {
        private readonly string? _apiKey;

        public YouTubeApiService(IConfiguration configuration)
        {
            _apiKey = configuration["YouTubeApiKey"] ?? throw new ArgumentNullException("YouTubeApiKey is missing in appsettings.json");
        }

        public async Task<List<YouTubeVideoModel>> SearchVideosAsync(string query, string duration, string uploadDate)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey,
                ApplicationName = "YoutubeProject"
            });

            var searchRequest = youtubeService.Search.List("snippet");
            searchRequest.Q = query;
            searchRequest.MaxResults = 10;
            searchRequest.Type = "video";

            // ✅ Fix duration filter
            if (!string.IsNullOrEmpty(duration) && duration != "any")
            {
                searchRequest.VideoDuration = duration switch
                {
                    "short" => SearchResource.ListRequest.VideoDurationEnum.Short__,
                    "medium" => SearchResource.ListRequest.VideoDurationEnum.Medium,
                    "long" => SearchResource.ListRequest.VideoDurationEnum.Long__,
                    _ => null
                };
            }

            // ✅ Fix uploadDate filter (use PublishedAfterDateTimeOffset)
            if (!string.IsNullOrEmpty(uploadDate) && uploadDate != "any")
            {
                searchRequest.PublishedAfterDateTimeOffset = uploadDate switch
                {
                    "today" => DateTimeOffset.UtcNow.AddDays(-1),
                    "this_week" => DateTimeOffset.UtcNow.AddDays(-7),
                    "this_month" => DateTimeOffset.UtcNow.AddMonths(-1),
                    _ => null
                };
            }

            var searchResponse = await searchRequest.ExecuteAsync();

            return searchResponse.Items.Select(item => new YouTubeVideoModel
            {
                Title = item.Snippet.Title,
                Description = item.Snippet.Description,
                ThumbnailUrl = item.Snippet.Thumbnails.Medium.Url,
                VideoUrl = "https://www.youtube.com/watch?v=" + item.Id.VideoId
            }).ToList();
        }
    }
}
