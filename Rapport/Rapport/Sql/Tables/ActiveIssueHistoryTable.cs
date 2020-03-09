using System;
using SQLite;

namespace Rapport.Sql.Tables
{
    public class ActiveIssueHistoryTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int TicketId { get; set; }

        public DateTime StartedUtc { get; set; }

        public DateTime FinishedUtc { get; set; }
    }
}
