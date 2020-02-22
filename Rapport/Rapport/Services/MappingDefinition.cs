using AutoMapper;

namespace Rapport.Services
{
    public static class MappingDefinition
    {

        public static IConfigurationProvider GetConfigurationProvider()
        {
            return new MapperConfiguration(cfg =>
            {

                // DTO <-> Model
                cfg.CreateMap<Data.DTO.Pexels.Photo, Data.Models.PhotoModel>()
                    .ForMember(src => src.Url, opt => opt.MapFrom(d => d.Src.Portrait));

                cfg.CreateMap<Data.DTO.Board, Data.Models.BoardModel>();
                cfg.CreateMap<Atlassian.Jira.Issue, Data.Models.IssueModel>();
                cfg.CreateMap<Data.DTO.Sprint, Data.Models.SprintModel>();

                // SQL Table <-> Model
                cfg.CreateMap<Sql.Tables.ActiveIssueTable, Data.Models.IssueModel>()
                   .ReverseMap();


            });
        }
    }
}
