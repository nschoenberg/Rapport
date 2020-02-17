using Prism.Navigation;

namespace Rapport.ViewModels
{
    public class OverviewPageViewModel : ViewModelBase
    {
        public OverviewPageViewModel() : base(null)
        {
            // Design Time Constructor
        }

        public OverviewPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Overview";
        }
    }
}
