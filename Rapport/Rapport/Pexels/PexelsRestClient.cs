using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Rapport.Data.DTO.Pexels;
using RestSharp;

namespace Rapport.Pexels
{
    public class PexelsRestClient : IPexelsRestClient
    {
        private readonly IRestClient _client;
        private readonly string _apiKey = string.Empty;
        private const string ApiBaseUri = "https://api.pexels.com/v1/";

        public PexelsRestClient()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rapport.Resources.secrets.txt"))
            {
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    var content = reader.ReadToEnd();
                    var indexOfSeparator = content.IndexOf(':');
                    if (indexOfSeparator > 0)
                    {
                        _apiKey = content.Substring(indexOfSeparator + 1);
                    }

                }
            }

            _client = new RestClient(ApiBaseUri);
        }

        public async Task<IRestResponse<SearchResponse>> SearchAsync(string query, int resultsPerPage = 15, int startPage = 1)
        {
            const string Resource = "search";
            const string QueryParameter = "query";
            const string PerPageParameter = "per_page";
            const string PageParameter = "page";

            var request = new RestRequest(Resource, Method.GET);
            request.AddHeader("Authorization", _apiKey);
            request.AddQueryParameter(QueryParameter, query);
            request.AddQueryParameter(PerPageParameter, resultsPerPage.ToString(CultureInfo.InvariantCulture));

            var normalizedStartPage = startPage < 1 ? 1 : startPage;

            request.AddQueryParameter(PageParameter, normalizedStartPage.ToString(CultureInfo.InvariantCulture));

            return await _client.ExecuteAsync<SearchResponse>(request).ConfigureAwait(false);
        }


    }
}
