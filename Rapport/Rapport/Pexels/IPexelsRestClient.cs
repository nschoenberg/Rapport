using System.Threading.Tasks;
using JetBrains.Annotations;
using Rapport.Data.DTO.Pexels;
using RestSharp;

namespace Rapport.Pexels
{
    public interface IPexelsRestClient
    {
        Task<IRestResponse<SearchResponse>> SearchAsync([NotNull] string query, int resultsPerPage = 15, int startPage = 1);
    }
}
