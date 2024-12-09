using Microsoft.EntityFrameworkCore;
using Vml.Core.Entities;

namespace Vml.Infrastructure.Data
{
    public class DataProcesserContext : DbContext
    {
        public DataProcesserContext(DbContextOptions<DataProcesserContext> options)
            : base(options)
        {
        }

        public DbSet<DataJob> DataJobs { get; set; }
    }
}
