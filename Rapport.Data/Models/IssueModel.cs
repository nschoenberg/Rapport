using System.Collections.Generic;
using JetBrains.Annotations;

namespace Rapport.Data.Models
{
    public class IssueModel
    {
        private readonly List<WorklogModel> _worklogModels = new List<WorklogModel>();

        public string Key { get; set; }

        public string Summary { get; set; }

        public string JiraIdentifier { get; set; }

        [CanBeNull]
        public WorklogModel CurrentWorklog { get; private set; }

        [NotNull]
        [ItemNotNull]
        public IReadOnlyCollection<WorklogModel> Worklog => _worklogModels;

        [NotNull]
        public WorklogModel AddWorklog()
        {
            CurrentWorklog = new WorklogModel();
            _worklogModels.Add(CurrentWorklog);
            return CurrentWorklog;
        }
    }
}
