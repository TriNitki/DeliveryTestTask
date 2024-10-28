using Microsoft.EntityFrameworkCore;

namespace Delivery.Service.Infrastructure;

/// <summary>
/// Extension to <see cref="IServiceCollection"/> that adds data access configuration.
/// </summary>
public static class ServiceCollectionDataAccessExtension
{
    /// <summary>
    /// Db provider.
    /// </summary>
    private const string DbProvider = "Npgsql";

    /// <summary>
    /// Adds database context.
    /// </summary>
    /// <typeparam name="TContext"> Context </typeparam>
    /// <param name="services"> Service Collection </param>
    /// <param name="cfg"> Application Configuration </param>
    /// <returns> Service Collection </returns>
    public static IServiceCollection AddDataContext<TContext>(this IServiceCollection services, IConfiguration cfg)
        where TContext : DbContext
    {
        var connectionString = cfg.GetConnectionString(DbProvider) 
                               ?? throw new ArgumentNullException(null, "Data base connection string not specified");
        
        return services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });
    }

    /// <summary>
    /// Adds test database context.
    /// </summary>
    /// <typeparam name="TContext"> Context </typeparam>
    /// <param name="services"> Service Collection </param>
    /// <param name="connectionString"> Connection string </param>
    /// <returns> Service Collection </returns>
    public static IServiceCollection AddDataContext<TContext>(this IServiceCollection services, string connectionString)
        where TContext : DbContext
    {
        return services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });
    }
}
