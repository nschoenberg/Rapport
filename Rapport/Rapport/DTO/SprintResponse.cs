using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Rapport.DTO
{
    public partial class SprintResponse
    {
        [JsonProperty("maxResults")]
        public long MaxResults { get; set; }

        [JsonProperty("startAt")]
        public long StartAt { get; set; }

        [JsonProperty("isLast")]
        public bool IsLast { get; set; }

        [JsonProperty("values")]
        public List<Sprint> Sprints { get; set; }
    }

    public partial class Sprint
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTimeOffset EndDate { get; set; }

        [JsonProperty("originBoardId")]
        public long OriginBoardId { get; set; }

        [JsonProperty("goal")]
        public string Goal { get; set; }
    }
}
