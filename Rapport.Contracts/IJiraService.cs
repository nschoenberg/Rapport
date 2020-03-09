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


        /// <summary>
        /// Adds an issue to the tracking system.
        /// </summary>
        /// <param name="issue">The issue.</param>
        Task AddTrackedIssueAsync([NotNull] IssueModel issue);

        /// <summary>
        /// Gets all issues that has been added to the tracking system via <see cref="AddTrackedIssueAsync"/>.
        /// </summary>
        /// <returns>All tracked issues.</returns>
        [NotNull]
        [ItemNotNull]
        Task<IEnumerable<IssueModel>> GetTrackedIssuesAsync();

        /// <summary>
        /// Removes an issue from the tracking system that has been added to the tracking system via <see cref="AddTrackedIssueAsync"/>.
        /// </summary>
        /// <param name="issue">The issue.</param>
        /// <returns>True when the issue has been removed, otherwise false.</returns>
        Task<bool> RemoveTrackedIssueAsync([NotNull] IssueModel issue);


        /// <summary>
        /// Stops the time tracking for the given issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        Task EndWorkingOnIssueAsync([NotNull] IssueModel issue);

        /// <summary>
        /// Starts the time tracking for the given issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        Task BeginWorkingOnIssueAsync([NotNull] IssueModel issue);
    }
}
