using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using Prism.Commands;
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

        private ICommand _activateIssueCommand;
        public ICommand ActivateIssueCommand =>
            _activateIssueCommand ?? (_activateIssueCommand = new DelegateCommand<IssueModel>(ExecuteActivateIssueCommand));

        private void ExecuteActivateIssueCommand([NotNull] IssueModel model)
        {
            _ = _jiraService.TrackIssueAsync(model).ConfigureAwait(false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Prism, its never null.")]
        public override void Initialize(INavigationParameters parameters)
        {
            _board = parameters.GetValue<BoardModel>("model");
            base.Initialize(parameters);
        }

        protected override async Task RefreshAsync()
        {
            Issues.Clear();

            var sprint = await _jiraService.GetActiveSprint(_board).ConfigureAwait(true);
            var issues = await _jiraService.GetIssues(_board, sprint).ConfigureAwait(true);

            foreach (var issue in issues.OrderBy(i => i.JiraIdentifier))
            {
                Issues.Add(issue);
            }
        }
    }
}
