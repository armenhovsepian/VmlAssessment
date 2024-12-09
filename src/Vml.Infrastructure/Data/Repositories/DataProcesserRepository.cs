using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vml.Core.Dtos;
using Vml.Core.Entities;
using Vml.Core.Interfaces;

namespace Vml.Infrastructure.Data.Repositories
{
    public class DataProcesserRepository : IDataProcesserRepository
    {
        private readonly DataProcesserContext _context;
        public DataProcesserRepository(DataProcesserContext context)
        {
            _context = context;
        }

        public DataJob Create(DataJob dataJob)
        {
            _context.DataJobs.Add(dataJob);

            return dataJob;
        }

        public async Task Delete(Guid dataJobID, CancellationToken cancellationToken)
        {
            var toBeDeleted = await GetDataJob(dataJobID, cancellationToken);

            if (toBeDeleted != null)
            {
                _context.DataJobs.Remove(toBeDeleted);
            }
        }

        public async Task<DataJob> GetDataJob(Guid id, CancellationToken cancellationToken)
        {
            return await _context.DataJobs.FirstOrDefaultAsync(job => job.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsDataJob(Guid id, CancellationToken cancellationToken)
        {
            return await _context.DataJobs.AnyAsync(job => job.Id == id, cancellationToken);
        }

        public DataJob Update(DataJob dataJob)
        {
            _context.DataJobs.Update(dataJob);
            return dataJob;
        }


        public async Task Commit(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<DataJobDTO>> GetAllDataJobs(int pageNumebr, int pageSize, CancellationToken cancellationToken, Expression<Func<DataJob, bool>> predicate = null)
        {
            var query = _context.DataJobs.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate).AsQueryable();
            }

            return await query
                .Skip(pageNumebr - 1)
                .Take(pageSize)
                .Select(job => new DataJobDTO
                {
                    Id = job.Id,
                    Name = job.Name,
                    FilePathToProcess = job.FilePathToProcess,
                    Status = job.Status,
                    Links = job.Links.Select(link => new LinkDTO
                    {
                        Id = link.Id,
                        Action = link.Action,
                        Rel = link.Rel,
                        Href = link.Href
                    })
                })
                .ToListAsync(cancellationToken);
        }
    }
}
