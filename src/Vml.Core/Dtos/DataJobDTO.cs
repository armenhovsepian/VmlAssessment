using Vml.Core.Enums;

namespace Vml.Core.Dtos
{
    public class DataJobDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FilePathToProcess { get; set; }
        public DataJobStatus Status { get; set; } = DataJobStatus.New;
        public IEnumerable<string> Results { get; set; } = new List<string>();
        public IEnumerable<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
