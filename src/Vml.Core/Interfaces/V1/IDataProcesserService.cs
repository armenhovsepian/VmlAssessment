using Ardalis.Result;
using Vml.Core.Dtos;
using Vml.Core.Enums;

namespace Vml.Core.Interfaces.V1
{
    public interface IDataProcesserService
    {
        Task<List<DataJobDTO>> GetAllDataJobs(int pageNumebr, int pageSize, CancellationToken cancellationToken);
        Task<List<DataJobDTO>> GetDataJobsByStatus(int pageNumebr, int pageSize, DataJobStatus status, CancellationToken cancellationToken);
        Task<Result<DataJobDTO>> GetDataJob(Guid id, CancellationToken cancellationToken);
        Task<Result<DataJobDTO>> Create(DataJobDTO dataJob, CancellationToken cancellationToken);
        Task<Result<DataJobDTO>> Update(DataJobDTO dataJob, CancellationToken cancellationToken);
        Task<Result> Delete(Guid dataJobID, CancellationToken cancellationToken);
        Task<Result<bool>> StartBackgroundProcess(Guid dataJobId, CancellationToken cancellationToken);
        Task<Result<DataJobStatus>> GetBackgroundProcessStatus(Guid dataJobId, CancellationToken cancellationToken);
        Task<List<string>> GetBackgroundProcessResults(Guid dataJobId, CancellationToken cancellationToken);
    }
}
