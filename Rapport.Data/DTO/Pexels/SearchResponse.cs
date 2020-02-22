using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rapport.Data.DTO.Pexels
{

    public class SearchResponse
    {
        [JsonProperty("total_results")]
        public long TotalResults { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("per_page")]
        public long PerPage { get; set; }

        [JsonProperty("photos")]
        public List<Photo> Photos { get; set; }

        [JsonProperty("next_page")]
        public Uri NextPage { get; set; }
    }

    public class Photo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("photographer")]
        public string Photographer { get; set; }

        [JsonProperty("photographer_url")]
        public Uri PhotographerUrl { get; set; }

        [JsonProperty("photographer_id")]
        public long PhotographerId { get; set; }

        [JsonProperty("src")]
        public Src Src { get; set; }

        [JsonProperty("liked")]
        public bool Liked { get; set; }
    }

    public class Src
    {
        [JsonProperty("original")]
        public Uri Original { get; set; }

        [JsonProperty("large2x")]
        public Uri Large2X { get; set; }

        [JsonProperty("large")]
        public Uri Large { get; set; }

        [JsonProperty("medium")]
        public Uri Medium { get; set; }

        [JsonProperty("small")]
        public Uri Small { get; set; }

        [JsonProperty("portrait")]
        public Uri Portrait { get; set; }

        [JsonProperty("landscape")]
        public Uri Landscape { get; set; }

        [JsonProperty("tiny")]
        public Uri Tiny { get; set; }
    }

}
