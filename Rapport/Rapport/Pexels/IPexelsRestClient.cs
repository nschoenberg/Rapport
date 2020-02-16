using System.Threading.Tasks;
using JetBrains.Annotations;
using Rapport.DTO.Pexels.Rapport.DTO;
using RestSharp;

namespace Rapport.Pexels
{
    public interface IPexelsRestClient
    {
        Task<IRestResponse<SearchResponse>> SearchAsync([NotNull] string query, byte resultsPerPage = 15, int startPage = 1);
    }
}
