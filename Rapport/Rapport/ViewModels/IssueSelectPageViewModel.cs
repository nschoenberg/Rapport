using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Atlassian.Jira;
using Prism.Navigation;
using Rapport.Contracts;
using Rapport.Data.DTO;

namespace Rapport.ViewModels
{
    public class IssueSelectPageViewModel : RefreshViewModelBase
    {
        private readonly IJiraService _jiraService;
        private Board _board;

        public IssueSelectPageViewModel() : base(null)
        {
            // Design Time constructor
        }

        public IssueSelectPageViewModel(
            INavigationService navigationService,
            IJiraService jiraService) : base(navigationService)
        {
            _jiraService = jiraService;
        }

        public IList<Data.DTO.IssueSmall> Issues { get; } = new ObservableCollection<Data.DTO.IssueSmall>();

        public override void Initialize(INavigationParameters parameters)
        {
            _board = parameters.GetValue<Board>("model");
            base.Initialize(parameters);
        }

        protected override async Task RefreshAsync()
        {
            Issues.Clear();

            var sprint = await _jiraService.GetActiveSprint(_board);
            var issues = await _jiraService.GetIssues(_board, sprint);

            foreach (var issue in issues)
            {
                Issues.Add(issue);
            }
        }
    }
}
