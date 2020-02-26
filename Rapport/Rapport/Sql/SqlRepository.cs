using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Rapport.Data.Models;
using Rapport.Sql.Tables;
using SQLite;
using Xamarin.Essentials.Interfaces;

namespace Rapport.Sql
{
    public interface ISqlRepository
    {
        Task SaveAsync(IssueModel issue);

        Task<IEnumerable<IssueModel>> GetAllAsync();
        Task<bool> DeleteAsync(IssueModel issue);
    }

    public class SqlRepository : ISqlRepository
    {
        private readonly SQLiteAsyncConnection _connection;
        private readonly IMapper _mapper;

        public SqlRepository(IFileSystem fileSystem, IConfigurationProvider mapperConfigurationProvider)
        {
            _connection = new SQLiteAsyncConnection(fileSystem.AppDataDirectory + "/rapport.db");
            _connection.CreateTableAsync<ActiveIssueTable>().ConfigureAwait(false);
            _connection.CreateTableAsync<ActiveIssueHistoryTable>().ConfigureAwait(false);
            _mapper = new Mapper(mapperConfigurationProvider);
        }

        public async Task SaveAsync(IssueModel issue)
        {
            var existingIssue = await _connection
                .Table<ActiveIssueTable>()
                .FirstOrDefaultAsync(x => x.Key == issue.Key)
                .ConfigureAwait(false);

            var issueRecord = _mapper.Map<IssueModel, ActiveIssueTable>(issue);

            if (existingIssue == null)
            {
                await _connection
                    .InsertAsync(issueRecord, typeof(ActiveIssueTable))
                    .ConfigureAwait(false);
            }
            else
            {
                issueRecord.Id = existingIssue.Id;
                await _connection.UpdateAsync(issueRecord, typeof(ActiveIssueTable));
            }
        }

        public async Task<IEnumerable<IssueModel>> GetAllAsync()
        {
            var activeIssues = await _connection.Table<ActiveIssueTable>().ToListAsync();
            return _mapper.Map<IEnumerable<ActiveIssueTable>, IEnumerable<IssueModel>>(activeIssues);
        }

        public async Task<bool> DeleteAsync(IssueModel issue)
        {
            var existingIssue = await _connection
                .Table<ActiveIssueTable>()
                .FirstOrDefaultAsync(x => x.Key == issue.Key)
                .ConfigureAwait(false);

            if (existingIssue != null)
            {
                return await _connection
                    .DeleteAsync<ActiveIssueTable>(existingIssue.Id)
                    .ConfigureAwait(false) > 0;
            }

            return false;
        }
    }
}
