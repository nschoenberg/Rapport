using System;
using JetBrains.Annotations;
using Prism.Mvvm;

namespace Rapport.Models
{
    public class PhotoModel : BindableBase
    {
        public static PhotoModel Empty = new PhotoModel();

        public PhotoModel()
        {
            Photographer = string.Empty;
        }

        public string Photographer { get; set; }

        [CanBeNull]
        public Uri PhotographerUrl { get; set; }

        [CanBeNull]
        public Uri Url { get; set; }
    }
}
