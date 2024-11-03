using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using repository.module.Models.Internal;

namespace repository.module.Configs
{
    internal class EventDataConfig : IEntityTypeConfiguration<EventDataInternal>
    {
        public void Configure(EntityTypeBuilder<EventDataInternal> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.Event)
                .WithOne(e => e.Data)
                .HasForeignKey<EventDataInternal>(d => d.EventId);
        }
    }
}
