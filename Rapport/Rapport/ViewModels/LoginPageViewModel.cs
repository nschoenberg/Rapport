using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Rapport.Contracts;
using Rapport.Services;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace Rapport.ViewModels
{
    public class LoginPageViewModel : ViewModelBase, IInitialize
    {
        private readonly IImageService _imageService;
        private readonly IPreferences _preferences;

        private ImageSource _backgroundImage;

        private bool _canExecuteRefresh;

        private bool _isRefreshing;

        private string _photographer;

        private ICommand _refreshBackgroundImageCommand;

        private string _userName;

        private string _userPassword;

        public LoginPageViewModel() : base(null)
        {
            // Design Time Constructor
        }

        public LoginPageViewModel(
            IImageService imageService,
            IPreferences preferences,
            INavigationService navigationService) : base(navigationService)
        {
            _imageService = imageService;
            _preferences = preferences;
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

        private ICommand _loginCommand;
        public ICommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand(ExecuteLoginCommand));

        private void ExecuteLoginCommand()
        {
            _preferences.Set("user", UserName);
            _preferences.Set("password", UserPassword);
            NavigationService.NavigateAsync(Pages.Home, ("user", UserName), ("password", UserPassword));
        }

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
                SetProperty(ref _isRefreshing, value);
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            UserName = _preferences.Get("user", string.Empty);
            UserPassword = _preferences.Get("password", string.Empty);

            _ = RefreshBackgroundImageAsync().ConfigureAwait(false);
        }


        private void ExecuteRefreshBackgroundCommand()
        {
            _ = RefreshBackgroundImageAsync().ConfigureAwait(false);
        }

        private async Task RefreshBackgroundImageAsync()
        {
            if (IsRefreshing == false)
            {
                IsRefreshing = true;
                CanExecuteRefresh = false;

                _ = Task.Run(async () =>
                {
                    await Task.Delay(3000).ConfigureAwait(true);
                    CanExecuteRefresh = true;
                    return Task.CompletedTask;
                });

                var photoModel = await _imageService
                    .GetRandomPhotoAsync()
                    .ConfigureAwait(true);

                BackgroundImageSource = photoModel.Url;
                Photographer = photoModel.Photographer;
                IsRefreshing = false;
            }
        }
    }
}
