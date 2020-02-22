using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Rapport.Data.DTO.Pexels;
using Rapport.Data.Models;
using Rapport.Pexels;
using Xamarin.Essentials.Interfaces;

namespace Rapport.Services
{
    public class ImageService : IImageService
    {
        private readonly IPexelsRestClient _restClient;
        private readonly Mapper _mapper;
        private readonly Random _random;

        public ImageService(
            IPexelsRestClient restClient,
            IConfigurationProvider mappingConfigurationProvider)
        {
            _restClient = restClient;
            _mapper = new Mapper(mappingConfigurationProvider);
            _random = new Random();
        }

        public async Task<PhotoModel> GetRandomPhotoAsync()
        {
            var randomPageIndex = _random.Next(1, 400);
            var response = await _restClient.SearchAsync("color:black", 20, randomPageIndex).ConfigureAwait(false);

            if (response.IsSuccessful == false)
            {
                return PhotoModel.Empty;
            }

            if (response.Data.Photos.Any() == false)
            {
                return PhotoModel.Empty;
            }

            var randomIndex = _random.Next(0, response.Data.Photos.Count - 1);
            var photo = response.Data.Photos[randomIndex];

            return _mapper.Map<Photo, PhotoModel>(photo);
        }
    }
}
