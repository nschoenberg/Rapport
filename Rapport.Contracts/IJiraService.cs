using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Rapport.Data.Models;

namespace Rapport.Contracts
{
    public interface IJiraService
    {
        void Initialize([NotNull] string userName, [NotNull] string userPassword);

        [NotNull]
        Task<IEnumerable<BoardModel>> GetAllBoardsAsync();

        [NotNull]
        Task<SprintModel> GetActiveSprint([NotNull] BoardModel board);

        [NotNull]
        Task<IEnumerable<IssueModel>> GetIssues([NotNull] BoardModel board, [NotNull] SprintModel sprint);

        Task TrackIssueAsync([NotNull] IssueModel issue);

        [NotNull]
        [ItemNotNull]
        Task<IEnumerable<IssueModel>> GetTrackedIssuesAsync();
    }
}
