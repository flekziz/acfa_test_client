using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using repository.module.Implementations;
using repository.module.Interfaces;

namespace repository.module
{
    public static class RepositoryDi
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(RepositoryDi))
                .DbConfigure(configuration)
                .AddScoped<IConfigurationRepository, ConfigurationRepository>()
                .AddScoped<IEventRepository, EventRepository>();

            return services;
        }

        private static IServiceCollection DbConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SQLDbConnection");
            services.AddDbContextFactory<AppDbContext>(opt => opt.UseNpgsql(connectionString));

            return services;
        }
    }
}
