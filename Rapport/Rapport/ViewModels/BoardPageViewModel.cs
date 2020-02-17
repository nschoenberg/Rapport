using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Atlassian.Jira;
using Prism.Navigation;
using Rapport.Contracts;
using Rapport.Data.DTO;

namespace Rapport.ViewModels
{
    public class BoardPageViewModel : RefreshViewModelBase, IInitialize
    {
        private readonly IJiraService _jiraService;
        private Jira _jira;
        private readonly ObservableCollection<Board> _boards = new ObservableCollection<Board>();

        public BoardPageViewModel() : base(null)
        {
            // Design Time constructor
        }

        public BoardPageViewModel(
            INavigationService navigationService,
            IJiraService jiraService)
            : base(navigationService)
        {
            _jiraService = jiraService;
            Title = "Select Board";
        }

        public override void Initialize(INavigationParameters parameters)
        {
            parameters.TryGetValue("user", out string user);
            parameters.TryGetValue("password", out string password);

            _jiraService.Initialize(user, password);

            base.Initialize(parameters);
        }

        public IList<Board> Boards
        {
            get { return _boards; }
        }

        protected override async Task RefreshAsync()
        {
            _boards.Clear();

            var boards = await _jiraService.GetAllBoardsAsync();

            foreach (var board in boards.OrderBy(b => b.Name))
            {
                _boards.Add(board);
            }

            // var issue = await _jira.Issues.GetIssueAsync("IAPP-726");
        }


    }
}
