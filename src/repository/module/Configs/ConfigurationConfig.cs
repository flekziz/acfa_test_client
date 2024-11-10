using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using repository.module.Models.Internal;

namespace repository.module.Configs
{
    internal class ConfigurationConfig : IEntityTypeConfiguration<ConfigurationInternal>
    {
        public void Configure(EntityTypeBuilder<ConfigurationInternal> builder)
        {
            builder.HasKey(c => c.Uid);

            builder.HasOne(c => c.Parent)
                .WithMany(c => c.Configurations)
                .HasForeignKey(c => c.ParentUid)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsMany(c => c.Properties, p => p.ToJson());
        }
    }
}
