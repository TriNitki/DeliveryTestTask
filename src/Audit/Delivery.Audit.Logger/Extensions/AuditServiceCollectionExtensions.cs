using Microsoft.Extensions.DependencyInjection;

namespace Delivery.Audit.Logger.Extensions;

public static class AuditServiceCollectionExtensions
{
    public static IServiceCollection AddAuditLogging(this IServiceCollection services)
    {
        services.AddTransient<AuditTarget>();
        services.AddSingleton<IAuditLogger, AuditLogger>();
        return services;
    }
}