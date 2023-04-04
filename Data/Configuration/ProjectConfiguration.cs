using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectTimer.Entities;
using ProjectTimer.Areas.Identity.Data;

namespace ProjectTimer.Data.Configuration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            //Name
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(50);
        }
    }
}


