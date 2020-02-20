using System.Threading.Tasks;
using Rapport.Data.Models;

namespace Rapport.Services
{
    public interface IImageService
    {
        Task<PhotoModel> GetRandomPhotoAsync();
    }
}
