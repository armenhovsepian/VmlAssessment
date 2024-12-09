using Ardalis.Result;
using Vml.Core.Enums;
using Vml.Core.Exceptions;

namespace Vml.Core.Entities
{
    public class DataJob
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string FilePathToProcess { get; private set; }
        public DataJobStatus Status { get; private set; } = DataJobStatus.New;
        //public IEnumerable<string> Results { get; private set; } = new List<string>();

        private readonly List<Link> _links = new List<Link>();
        public IReadOnlyCollection<Link> Links => _links.AsReadOnly();

        private DataJob()
        {

        }


        public DataJob(string name, string filePathToProcess)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new DataJobException($"{nameof(Name)} of DataJob can not be Nul or Empty", new ArgumentNullException(nameof(Name)));
            }

            Name = name;
            FilePathToProcess = filePathToProcess;
            Status = DataJobStatus.New;
        }

        public Result UpdateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result.Invalid(new ValidationError { ErrorMessage = $"{nameof(Name)} can not be null or Empty." });
            }

            Name = name;
            return Result.Success();
        }

        public Result UpdateFilePathToProcess(string filePathToProcess)
        {
            if (string.IsNullOrEmpty(filePathToProcess))
            {
                return Result.Invalid(new ValidationError { ErrorMessage = $"{nameof(FilePathToProcess)} can not be null or Empty." });
            }

            FilePathToProcess = filePathToProcess;
            return Result.Success();
        }


        public void AddLink(Link link)
        {
            if (!_links.Contains(link))
            {
                _links.Add(link);
            }
        }

        public Result SetCompleted()
        {
            if (Status == DataJobStatus.Completed)
            {
                return Result.Forbidden("dataJob has completed already");
            }

            Status = DataJobStatus.Completed;

            return Result.Success();
        }

        public Result SetProcessing()
        {
            if (Status == DataJobStatus.Completed)
            {
                return Result.Forbidden("Can not change status of completed dataJob");
            }

            Status = DataJobStatus.Processing;

            return Result.Success();
        }

        public void ClearLinks()
        {
            _links.Clear();
        }
    }
}
