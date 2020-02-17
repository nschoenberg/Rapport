using System.Collections.Generic;
using System.Threading.Tasks;
using Atlassian.Jira;
using JetBrains.Annotations;
using Rapport.Data.DTO;

namespace Rapport.Contracts
{
    public interface IJiraService
    {
        void Initialize([NotNull] string userName, [NotNull] string userPassword);

        [NotNull]
        Task<IEnumerable<Board>> GetAllBoardsAsync();

        [NotNull]
        Task<Sprint> GetActiveSprint([NotNull] Board board);

        [NotNull]
        Task<IEnumerable<Issue>> GetIssues([NotNull] Board board, [NotNull] Sprint sprint);
    }
}
