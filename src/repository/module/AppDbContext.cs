using Microsoft.EntityFrameworkCore;
using repository.module.Configs;
using repository.module.Models.Internal;

namespace repository.module
{
    internal class AppDbContext : DbContext
    {
        public DbSet<EventInternal> Events => Set<EventInternal>();
        public DbSet<EventDataInternal> EventsData => Set<EventDataInternal>();
        public DbSet<ConfigurationInternal> Configurations => Set<ConfigurationInternal>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigurationConfig())
                .ApplyConfiguration(new EventConfig())
                .ApplyConfiguration(new EventDataConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
