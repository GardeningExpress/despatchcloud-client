using System.Collections.Generic;
using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient.DTO
{
    public class PagedResult<T>
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("per_page")]
        public string PerPage { get; set; }

        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }

        [JsonProperty("last_page")]
        public int LastPage { get; set; }

        [JsonProperty("next_page_url")]
        public string NextPageUrl { get; set; }

        [JsonProperty("prev_page_url")]
        public object PreviousPageUrl { get; set; }

        [JsonProperty("from")]
        public int From { get; set; }

        [JsonProperty("to")]
        public int To { get; set; }

        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }
}