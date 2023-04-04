using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTimer.Areas.Identity.Data;

namespace ProjectTimer.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Name
            //builder.Property(p => p.Name).IsRequired();
            //builder.Property(p => p.Name).HasMaxLength(50);
        }
    }
}
