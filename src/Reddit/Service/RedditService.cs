using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Reddit.Model;
using Reddit.Service.Contracts;

namespace Reddit.Service
{
    internal class RedditService : IRedditService
    {
        private readonly string _channelUrl = "https://www.reddit.com/top";
        private readonly HttpClient _client = new HttpClient();
        private readonly ILogger _logger;
        private readonly int _pageLimit = 10;

        public RedditService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<List<RedditItem>> GetPage(string afterId = null)
        {
            return await GetPage(_pageLimit, afterId);
        }

        public async Task<List<RedditItem>> GetPage(int count, string afterId = null)
        {
            var result = new List<RedditItem>();

            _logger.Log($"------Reading new 10 elements [{afterId}]------");

            var url = $"{_channelUrl}/.json?limit={count}&after={afterId}";

            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = ParseResponse(content);
            }

            return result;
        }

        private List<RedditItem> ParseResponse(string content)
        {
            var result = new List<RedditItem>();

            try
            {
                var json = JObject.Parse(content);
                var childs = (JArray) json["data"]["children"];

                foreach (var child in childs)
                {
                    var item = child["data"].ToObject<RedditItem>();
                    result.Add(item);
                }
            }
            catch
            {
                _logger.Log("Error parsing page...");
            }

            return result;
        }
    }
}