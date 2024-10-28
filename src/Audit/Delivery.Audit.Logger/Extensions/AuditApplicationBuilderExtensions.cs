using Microsoft.AspNetCore.Builder;
using NLog.Web;

namespace Delivery.Audit.Logger.Extensions;

public static class AuditApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAuditLogging(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<NLogRequestPostedBodyMiddleware>(
            new NLogRequestPostedBodyMiddlewareOptions());
        return builder;
    }
}