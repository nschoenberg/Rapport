using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Prism.Navigation;
using Rapport.Contracts;
using Rapport.Data.Models;

namespace Rapport.ViewModels
{
    public class BoardPageViewModel : RefreshViewModelBase
    {
        private readonly IJiraService _jiraService;
        private readonly ObservableCollection<BoardModel> _boards = new ObservableCollection<BoardModel>();

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Prism, its never null.")]
        public override void Initialize([NotNull] INavigationParameters parameters)
        {
            parameters.TryGetValue("user", out string user);
            parameters.TryGetValue("password", out string password);

            _jiraService.Initialize(user, password);

            base.Initialize(parameters);
        }

        public IList<BoardModel> Boards
        {
            get { return _boards; }
        }

        protected override async Task RefreshAsync()
        {
            Boards.Clear();

            var boards = await _jiraService.GetAllBoardsAsync().ConfigureAwait(true);

            foreach (var board in boards.OrderBy(b => b.Name))
            {
                Boards.Add(board);
            }
        }
    }
}
