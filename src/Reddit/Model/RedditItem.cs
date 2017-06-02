using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Reddit.Utilities;

namespace Reddit.Model
{
    internal class RedditItem
    {
        [JsonProperty("name")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(UnixDateJsonConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("num_comments")]
        public int Comments { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("preview")]
        public RedditPreview Preview { get; set; }
    }

    internal class RedditPreview
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("images")]
        public List<RedditImage> Images { get; set; }
    }

    internal class RedditImage
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("source")]
        public RedditImageSource Source { get; set; }
    }

    internal class RedditImageSource
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}