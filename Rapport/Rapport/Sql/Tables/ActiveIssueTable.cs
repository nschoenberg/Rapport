using SQLite;

namespace Rapport.Sql.Tables
{
    public class ActiveIssueTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique, Indexed]
        public string Key { get; set; }

        public string Summary { get; set; }

        public string JiraIdentifier { get; set; }
    }
}
