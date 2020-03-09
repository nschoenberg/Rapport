using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlassian.Jira;
using AutoMapper;
using JetBrains.Annotations;
using Rapport.Contracts;
using Rapport.Data.DTO;
using Rapport.Data.Models;
using Rapport.Sql;
using RestSharp;
using IFileSystem = Xamarin.Essentials.Interfaces.IFileSystem;

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
        private readonly ISqlRepository _sqlRepository;
        private Jira _jira;
        private readonly IMapper _mapper;

        public JiraService(
            IConfigurationProvider mappingConfigurationProvider,
            ISqlRepository sqlRepository)
        {
            _sqlRepository = sqlRepository;
            _mapper = new Mapper(mappingConfigurationProvider);
        }

        public void Initialize(string userName, string userPassword)
        {
            _jira = Jira.CreateRestClient("https://intranet.futura4retail.com/issues/", userName, userPassword);
        }


        /// <summary>
        /// https://docs.atlassian.com/jira-software/REST/7.0.4/?_ga=2.39551655.524931630.1581610578-564421449.1549631859#agile/1.0/board-getAllBoards
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        public async Task<IEnumerable<BoardModel>> GetAllBoardsAsync()
        {
            EnsureInitialized();

            var boards = new List<Board>();

            try
            {
                var boardResponse =
                    await _jira
                        .RestClient
                        .ExecuteRequestAsync<BoardResponse>(Method.GET, "rest/agile/1.0/board?startAt=0&type=scrum")
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

            return _mapper.Map<IEnumerable<Board>, IEnumerable<BoardModel>>(boards);
        }

        public async Task<SprintModel> GetActiveSprint(BoardModel board)
        {
            EnsureInitialized();

            var sprint = Sprint.Empty;

            try
            {
                var sprintResponse = await _jira
                    .RestClient
                    .ExecuteRequestAsync<SprintResponse>(Method.GET, "rest/agile/1.0/board/" + board.Id + "/sprint?state=active")
                    .ConfigureAwait(false);

                sprint = sprintResponse.Sprints.First();
            }
            catch (Exception e)
            {
                // TODO Logging
            }

            return _mapper.Map<SprintModel>(sprint);
        }

        public async Task<IEnumerable<IssueModel>> GetIssues(BoardModel board, SprintModel sprint)
        {
            EnsureInitialized();

            var issues = Enumerable.Empty<Issue>();

            try
            {
                var issuesResponse = await _jira
                    .RestClient
                    .ExecuteRequestAsync<IssueResponse>(Method.GET, "rest/agile/1.0/board/" + board.Id + "/sprint/" + sprint.Id + "/issue")
                    .ConfigureAwait(false);

                var keys = issuesResponse.Issues.Select(i => "\"" + i.Key + "\"");
                var keysFilter = "(" + string.Join(",", keys) + ")";
                var jql = "key in " + keysFilter;

                issues = await _jira.Issues.GetIssuesFromJqlAsync(jql);

            }
            catch (Exception e)
            {
                // TODO Log
            }

            return _mapper.Map<IEnumerable<Issue>, IEnumerable<IssueModel>>(issues);
        }

        public async Task AddTrackedIssueAsync(IssueModel issue)
        {
            await _sqlRepository
                .SaveAsync(issue)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<IssueModel>> GetTrackedIssuesAsync()
        {
            var trackedIssues = await _sqlRepository.GetAllAsync().ConfigureAwait(false);
            return trackedIssues;
        }

        public async Task<bool> RemoveTrackedIssueAsync(IssueModel issue)
        {
            return await _sqlRepository
                .DeleteAsync(issue)
                .ConfigureAwait(false);
        }

        public Task EndWorkingOnIssueAsync(IssueModel issue)
        {
            issue.CurrentWorklog?.Finish();
            return Task.CompletedTask;
        }

        public async Task BeginWorkingOnIssueAsync(IssueModel issue)
        {
            issue.AddWorklog();
            await _sqlRepository.SaveAsync(issue).ConfigureAwait(false);
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
