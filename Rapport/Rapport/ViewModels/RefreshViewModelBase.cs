using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;

namespace Rapport.ViewModels
{
    public abstract class RefreshViewModelBase : ViewModelBase
    {
        private bool _isRefreshing;

        protected RefreshViewModelBase(INavigationService navigationService) : base(navigationService) { }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        private DelegateCommand _refreshCommand;
        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new DelegateCommand(() => _ = ExecuteRefreshCommandAsync(), () => !IsRefreshing).ObservesProperty(() => IsRefreshing));

        public override void Initialize(INavigationParameters parameters)
        {
            _ = ExecuteRefreshCommandAsync();
        }

        private async Task ExecuteRefreshCommandAsync()
        {
            IsRefreshing = true;
            await RefreshAsync().ConfigureAwait(false);
            IsRefreshing = false;
        }

        protected abstract Task RefreshAsync();

    }
}
