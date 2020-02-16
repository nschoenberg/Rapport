using System.Threading.Tasks;
using Rapport.Models;

namespace Rapport.Services
{
    public interface IImageService
    {
        Task<PhotoModel> GetRandomPhotoAsync();
    }
}
