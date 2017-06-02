using Newtonsoft.Json;

namespace Reddit.Model
{
    internal class MainViewModelState
    {
        [JsonProperty("itemsCount")]
        public int ItemsCount { get; set; }

        [JsonProperty("lastId")]
        public string LastId { get; set; }
    }
}