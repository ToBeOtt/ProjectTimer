using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTimer.Entities;

namespace ProjectTimer.Data.Configuration
{
    public class ClockConfiguration : IEntityTypeConfiguration<Clock>
    {
        public void Configure(EntityTypeBuilder<Clock> builder)
        {
            //Description
            builder.Property(c => c.Description).HasMaxLength(500);
            
        }

    }
}