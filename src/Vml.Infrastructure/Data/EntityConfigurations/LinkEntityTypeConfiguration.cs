using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vml.Core.Entities;

namespace Vml.Infrastructure.Data.EntityConfigurations
{
    public class LinkEntityTypeConfiguration : IEntityTypeConfiguration<Link>
    {

        public void Configure(EntityTypeBuilder<Link> builder)
        {
            builder
                .Property(b => b.Rel)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(b => b.Href)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(b => b.Action)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
