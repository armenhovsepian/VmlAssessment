using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vml.Core.Entities;

namespace Vml.Infrastructure.Data.EntityConfigurations
{
    public class DataJobEntityTypeConfiguration : IEntityTypeConfiguration<DataJob>
    {

        public void Configure(EntityTypeBuilder<DataJob> builder)
        {
            builder
                .Property(b => b.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(b => b.FilePathToProcess)
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}
