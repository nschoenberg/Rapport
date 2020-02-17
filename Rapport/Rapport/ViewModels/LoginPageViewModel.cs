using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Rapport.Services;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace Rapport.ViewModels
{
    public class LoginPageViewModel : ViewModelBase, IInitialize
    {
        private readonly IImageService _imageService;

        private ImageSource _backgroundImage;

        private bool _canExecuteRefresh;

        private bool _isRefreshing;

        private string _pexelLink;

        private string _photographer;

        private ICommand _refreshBackgroundImageCommand;

        private string _userName;

        private string _userPassword;

        public LoginPageViewModel() : base(null)
        {
            // Design Time Constructor
        }

        public LoginPageViewModel(IDeviceDisplay deviceDisplay, IImageService imageService,
            INavigationService navigationService) : base(navigationService)
        {
            _imageService = imageService;
            //var uri = new Uri(
            //    "https://images.pexels.com/photos/1076429/pexels-photo-1076429.jpeg?auto=compress&cs=tinysrgb&dpr=3&h=250&w=1260");

            //uri = new Uri("https://images.pexels.com/photos/397998/pexels-photo-397998.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260");


            //var width = deviceDisplay.MainDisplayInfo.Width;
            //var height = deviceDisplay.MainDisplayInfo.Height;
            //uri = new Uri($"https://images.pexels.com/photos/604684/pexels-photo-604684.jpeg?auto=compress&cs=tinysrgb&dpr=3&h={height}&w={width}");

            //_backgroundImage = ImageSource.FromUri(uri);
        }

        public ImageSource BackgroundImageSource
        {
            get { return _backgroundImage; }
            set { SetProperty(ref _backgroundImage, value); }
        }

        public string Photographer
        {
            get { return _photographer; }
            set { SetProperty(ref _photographer, value); }
        }

        public ICommand RefreshBackgroundImageCommand =>
            _refreshBackgroundImageCommand ?? (_refreshBackgroundImageCommand =
                new DelegateCommand(ExecuteRefreshBackgroundCommand).ObservesCanExecute(() => CanExecuteRefresh));

        public bool CanExecuteRefresh
        {
            get { return _canExecuteRefresh; }
            set { SetProperty(ref _canExecuteRefresh, value); }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public string UserPassword
        {
            get { return _userPassword; }
            set { SetProperty(ref _userPassword, value); }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                var changed = SetProperty(ref _isRefreshing, value);
                if (changed && value) { }
            }
        }

        public void Initialize(INavigationParameters parameters)
        {
            _ = RefreshBackgroundImageAsync();
        }


        private void ExecuteRefreshBackgroundCommand()
        {
            _ = RefreshBackgroundImageAsync();
        }

        private async Task RefreshBackgroundImageAsync()
        {
            if (IsRefreshing == false)
            {
                IsRefreshing = true;
                CanExecuteRefresh = false;

                _ = Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    CanExecuteRefresh = true;
                    return Task.CompletedTask;
                });

                var photoModel = await _imageService.GetRandomPhotoAsync();

                BackgroundImageSource = photoModel.Url;
                Photographer = photoModel.Photographer;
                IsRefreshing = false;
            }
        }
    }
}
