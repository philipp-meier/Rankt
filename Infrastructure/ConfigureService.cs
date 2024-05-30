using Microsoft.EntityFrameworkCore;
using Rankt.Infrastructure.Persistence;
using Rankt.Shared.Audit;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"), builder =>
            {
                builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
            }));

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
    }
}
