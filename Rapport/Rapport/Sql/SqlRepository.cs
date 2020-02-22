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
        Task SaveAsync(IssueModel model);

        Task<IEnumerable<IssueModel>> GetAllAsync();
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

        public async Task SaveAsync(IssueModel model)
        {
            var existingIssue = await _connection
                .Table<ActiveIssueTable>()
                .FirstOrDefaultAsync(x => x.Key == model.Key)
                .ConfigureAwait(false);

            var issue = _mapper.Map<IssueModel, ActiveIssueTable>(model);

            if (existingIssue == null)
            {
                await _connection
                    .InsertAsync(issue, typeof(ActiveIssueTable))
                    .ConfigureAwait(false);
            }
            else
            {
                issue.Id = existingIssue.Id;
                await _connection.UpdateAsync(issue, typeof(ActiveIssueTable));
            }
        }

        public async Task<IEnumerable<IssueModel>> GetAllAsync()
        {
            var activeIssues = await _connection.Table<ActiveIssueTable>().ToListAsync();
            return _mapper.Map<IEnumerable<ActiveIssueTable>, IEnumerable<IssueModel>>(activeIssues);
        }
    }
}
