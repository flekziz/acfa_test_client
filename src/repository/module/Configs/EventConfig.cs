using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using repository.module.Models.Internal;

namespace repository.module.Configs
{
    internal class EventConfig : IEntityTypeConfiguration<EventInternal>
    {
        public void Configure(EntityTypeBuilder<EventInternal> builder)
        {
            builder.HasKey(e => e.Uid);

            builder.HasOne(e => e.Data)
                .WithOne(d => d.Event)
                .HasForeignKey<EventDataInternal>(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
