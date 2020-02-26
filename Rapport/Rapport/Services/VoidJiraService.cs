using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rapport.Contracts;
using Rapport.Data.Models;

namespace Rapport.Services
{
    public class VoidJiraService : IJiraService
    {
        public void Initialize(string userName, string userPassword)
        {
        }

        public Task<IEnumerable<BoardModel>> GetAllBoardsAsync()
        {
            return Task.FromResult(Enumerable.Empty<BoardModel>());
        }

        public Task<SprintModel> GetActiveSprint(BoardModel board)
        {
            return Task.FromResult(SprintModel.Empty);
        }

        public Task<IEnumerable<IssueModel>> GetIssues(BoardModel board, SprintModel sprint)
        {
            return Task.FromResult(Enumerable.Empty<IssueModel>());
        }

        public Task TrackIssueAsync(IssueModel issue)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<IssueModel>> GetTrackedIssuesAsync()
        {
            IEnumerable<IssueModel> res = new List<IssueModel>
            {
                new IssueModel { Key = "4711", Summary = "yo"},
                new IssueModel { Key = "4712", Summary = "yo2"},
            };

            return Task.FromResult(res);
        }

        public Task<bool> RemoveTrackedIssueAsync(IssueModel issue)
        {
            return Task.FromResult(true);
        }
    }
}
