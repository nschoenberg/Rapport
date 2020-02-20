using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using Rapport.Contracts;
using Rapport.Data.Models;

namespace Rapport.ViewModels
{
    public class IssueSelectPageViewModel : RefreshViewModelBase
    {
        private readonly IJiraService _jiraService;
        private BoardModel _board;

        public IssueSelectPageViewModel() : base(null)
        {
            // Design Time constructor
        }

        public IssueSelectPageViewModel(
            INavigationService navigationService,
            IJiraService jiraService) : base(navigationService)
        {
            Title = "Select Issues";
            _jiraService = jiraService;
        }

        public IList<IssueModel> Issues { get; } = new ObservableCollection<IssueModel>();

        public override void Initialize(INavigationParameters parameters)
        {
            _board = parameters.GetValue<BoardModel>("model");
            base.Initialize(parameters);
        }

        protected override async Task RefreshAsync()
        {
            Issues.Clear();

            var sprint = await _jiraService.GetActiveSprint(_board).ConfigureAwait(false);

            // TODO: Verify correct synchornization context is used here since Collection Issues is not thread safe and UI bound
            var issues = await _jiraService.GetIssues(_board, sprint).ConfigureAwait(true);

            foreach (var issue in issues.OrderBy(i => i.JiraIdentifier))
            {
                Issues.Add(issue);
            }
        }
    }
}
