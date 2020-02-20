using AutoMapper;

namespace Rapport.Services
{
    public class MappingDefinition
    {

        public static IConfigurationProvider GetConfigurationProvider()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Atlassian.Jira.Issue, Data.Models.IssueModel>();
            });
        }
    }
}
