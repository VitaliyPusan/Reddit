using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly int _pageLimit = 10;

        public async Task<List<RedditItem>> GetPage(string afterId = null)
        {
            var result = new List<RedditItem>();

            Debug.WriteLine($"------Reading new 10 elements [{afterId}]------");

            var url = $"{_channelUrl}/.json?limit={_pageLimit}&after={afterId}";

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
            catch (Exception ex)
            {
                Debug.WriteLine("Error parsing page...");
            }

            return result;
        }
    }
}