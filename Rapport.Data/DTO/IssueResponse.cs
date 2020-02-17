using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Rapport.Data.DTO
{

    public class IssueResponse
    {
        [JsonProperty("expand")] public string Expand { get; set; }

        [JsonProperty("startAt")] public long StartAt { get; set; }

        [JsonProperty("maxResults")] public long MaxResults { get; set; }

        [JsonProperty("total")] public long Total { get; set; }

        [JsonProperty("issues")] public List<IssueSmall> Issues { get; set; }
    }

    public class IssueSmall
    {
        [JsonProperty("expand")] public string Expand { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("self")] public Uri Self { get; set; }

        [JsonProperty("key")] public string Key { get; set; }

    }



}
