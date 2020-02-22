using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace Rapport.ViewModels
{
    public abstract class RefreshViewModelBase : ViewModelBase
    {
        private volatile bool _isRefreshing;

        protected RefreshViewModelBase(INavigationService navigationService) : base(navigationService) { }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new DelegateCommand(() => _ = ExecuteRefreshCommandAsync()));

        public override void Initialize(INavigationParameters parameters)
        {
            _ = ExecuteRefreshCommandAsync();
        }

        protected async Task ExecuteRefreshCommandAsync()
        {
            if (IsRefreshing == false)
            {
                IsRefreshing = true;
                await RefreshAsync();
                IsRefreshing = false;
            }
        }

        protected abstract Task RefreshAsync();

    }
}
