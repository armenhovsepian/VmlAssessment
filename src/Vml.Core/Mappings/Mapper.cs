using Vml.Core.Dtos;
using Vml.Core.Entities;

namespace Vml.Core.Mappings
{
    public static class Mapper
    {
        public static DataJobDTO ToDataJobDto(this DataJob dataJob)
        {
            return new DataJobDTO
            {
                Id = dataJob.Id,
                Name = dataJob.Name,
                FilePathToProcess = dataJob.FilePathToProcess,
                Links = dataJob.Links.Select(link => link.ToLinkDto()),
                //Results = dataJob.Results,
                Status = dataJob.Status,
            };
        }

        public static LinkDTO ToLinkDto(this Link link)
        {
            return new LinkDTO
            {
                Id = link.Id,
                Href = link.Href,
                Action = link.Action,
                Rel = link.Rel,
                Types = link.Types
            };
        }
    }
}
