using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Atlassian.Jira;
using Prism.Navigation;
using Rapport.DTO;
using RestSharp;

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

        public override void Initialize(INavigationParameters parameters)
        {
            _ = ExecuteRefreshCommandAsync();
        }

        protected async Task ExecuteRefreshCommandAsync()
        {
            IsRefreshing = true;
            await RefreshAsync();
            IsRefreshing = false;
        }

        protected abstract Task RefreshAsync();

    }

    public class MainPageViewModel : RefreshViewModelBase, IInitialize
    {
        private Jira _jira;
        private readonly ObservableCollection<Board> _boards = new ObservableCollection<Board>();

        public MainPageViewModel() : base(null)
        {
            // Design Time constructor
        }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {

            _jira = Jira.CreateRestClient("https://intranet.futura4retail.com/issues", "", "");
            //var issues = from i in _jira.Issues.Queryable
            //             where i.Project == "IAPP" && i.Reporter == "Nicolai Schönberg"
            //             orderby i.Created
            //             select i;

            //var x = issues.ToList();

            //// rest/agile/1.0/board
            //// rest/agile/1.0/sprint/632
            //// rest/agile/1.0/board/131/sprint?state=active

            //var result = _jira.RestClient.ExecuteRequestAsync<BoardResponse>(Method.GET, "rest/agile/1.0/board?startAt=50").Result;
            //var board = result.Boards.FirstOrDefault(b => b.Name == "Roqqio Instore App Board");
            //var sprints = _jira.RestClient.ExecuteRequestAsync<SprintResponse>(Method.GET, "rest/agile/1.0/board/" + board.Id + "/sprint?state=active").Result;

            //// https://docs.atlassian.com/jira-software/REST/7.0.4/?_ga=2.39551655.524931630.1581610578-564421449.1549631859#agile/1.0/board-getAllBoards
            //// use sdk: https://bitbucket.org/farmas/atlassian.net-sdk/src/770bc0e2747515327b7edabe0f8c7d2c1b967e5c/docs/how-to-use-the-sdk.md

            Title = "Select Board";
        }


        public IList<Board> Boards
        {
            get { return _boards; }
        }

        protected override async Task RefreshAsync()
        {
            var boards = await GetAllBoardsAsync();


            foreach (var board in boards.OrderBy(b => b.Name))
            {
                _boards.Add(board);
            }

            // var issue = await _jira.Issues.GetIssueAsync("IAPP-726");
        }

        private async Task<IEnumerable<Board>> GetAllBoardsAsync()
        {
            var boards = new List<Board>();

            var boardResponse =
                await _jira.RestClient.ExecuteRequestAsync<BoardResponse>(Method.GET, "rest/agile/1.0/board?startAt=0");

            boards.AddRange(boardResponse.Boards);


            while (boardResponse.IsLast == false && boards.Count < 200)
            {
                boardResponse =
                    await _jira.RestClient.ExecuteRequestAsync<BoardResponse>(Method.GET,
                        "rest/agile/1.0/board?startAt=" + boards.Count);
                boards.AddRange(boardResponse.Boards);
            }

            return boards;
        }
    }
}
