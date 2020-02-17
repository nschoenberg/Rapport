using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlassian.Jira;
using JetBrains.Annotations;
using Rapport.Contracts;
using Rapport.Data.DTO;

using RestSharp;

using Sprint = Rapport.Data.DTO.Sprint;

namespace Rapport.Services
{
    /// <summary>
    /// Abstracts jira rest requests.
    /// Further readings:
    /// https://docs.atlassian.com/jira-software/REST/7.0.4/?_ga=2.39551655.524931630.1581610578-564421449.1549631859#agile/1.0/board-getAllBoards
    /// https://bitbucket.org/farmas/atlassian.net-sdk/src/770bc0e2747515327b7edabe0f8c7d2c1b967e5c/docs/how-to-use-the-sdk.md
    /// </summary>
    public class JiraService : IJiraService
    {
        private Jira _jira;

        public void Initialize(string userName, string userPassword)
        {
            _jira = Jira.CreateRestClient("https://intranet.futura4retail.com/issues/", userName, userPassword);
        }

        [NotNull]
        [ItemNotNull]
        public async Task<IEnumerable<Board>> GetAllBoardsAsync()
        {
            EnsureInitialized();

            var boards = new List<Board>();

            try
            {
                var boardResponse =
                    await _jira
                        .RestClient
                        .ExecuteRequestAsync<BoardResponse>(Method.GET, "rest/agile/1.0/board?startAt=0")
                        .ConfigureAwait(false);

                boards.AddRange(boardResponse.Boards);

                while (boardResponse.IsLast == false && boards.Count < 200)
                {
                    boardResponse =
                        await _jira
                            .RestClient
                            .ExecuteRequestAsync<BoardResponse>(Method.GET, "rest/agile/1.0/board?startAt=" + boards.Count)
                            .ConfigureAwait(false);

                    boards.AddRange(boardResponse.Boards);
                }
            }
            catch (Exception e)
            {
                // TODO Log
            }

            return boards;
        }

        public async Task<Sprint> GetActiveSprint(Board board)
        {
            EnsureInitialized();

            try
            {
                var sprintResponse = await _jira
                    .RestClient
                    .ExecuteRequestAsync<SprintResponse>(Method.GET, "rest/agile/1.0/board/" + board.Id + "/sprint?state=active")
                    .ConfigureAwait(false);

                return sprintResponse.Sprints.First();
            }
            catch (Exception e)
            {
                // TODO Log
                return Sprint.Empty;
            }
        }

        public async Task<IEnumerable<Issue>> GetIssues(Board board, Sprint sprint)
        {
            EnsureInitialized();

            try
            {
                var issuesResponse = await _jira
                    .RestClient
                    .ExecuteRequestAsync<IssueResponse>(Method.GET, "rest/agile/1.0/board/" + board.Id + "/sprint/" + sprint.Id + "/issue")
                    .ConfigureAwait(false);

                var tasks = issuesResponse.Issues.Select(issue => _jira.Issues.GetIssueAsync(issue.Key));

                var issues = await Task.WhenAll(tasks).ConfigureAwait(false);

                return issues;
            }
            catch (Exception e)
            {
                // TODO Log
                return Enumerable.Empty<Issue>();
            }
        }

        private void EnsureInitialized()
        {
            if (_jira == null)
            {
                throw new InvalidOperationException("Service not initialized. Call Initialize once before any other method!");
            }
        }
    }
}
