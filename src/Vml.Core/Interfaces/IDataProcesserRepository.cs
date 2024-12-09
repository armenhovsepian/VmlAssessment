using System.Linq.Expressions;
using Vml.Core.Dtos;
using Vml.Core.Entities;

namespace Vml.Core.Interfaces
{
    public interface IDataProcesserRepository
    {
        Task<DataJob> GetDataJob(Guid id, CancellationToken cancellationToken);
        DataJob Create(DataJob dataJob);
        DataJob Update(DataJob dataJob);
        Task Delete(Guid dataJobID, CancellationToken cancellationToken);
        Task<bool> ExistsDataJob(Guid id, CancellationToken cancellationToken);
        Task Commit(CancellationToken cancellationToken);

        Task<List<DataJobDTO>> GetAllDataJobs(int pageNumebr, int pageSize, CancellationToken cancellationToken, Expression<Func<DataJob, bool>> predicate = null);

    }
}
