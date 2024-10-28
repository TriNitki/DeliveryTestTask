using Microsoft.AspNetCore.Builder;
using NLog.Web;

namespace Delivery.Logger.Extensions;

/// <summary>
/// Методы расширения <see cref="IApplicationBuilder"/> для интеграции журнала аудита.
/// </summary>
public static class AuditApplicationBuilderExtensions
{
    /// <summary>
    /// Добавить промежуточное ПО журнала аудита в конвейер обработки запросов.
    /// </summary>
    /// <param name="builder"> Конструктор приложений. </param>
    /// <returns> Конструктор приложений. </returns>
    public static IApplicationBuilder UseAuditLogging(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<NLogRequestPostedBodyMiddleware>(
            new NLogRequestPostedBodyMiddlewareOptions());
        return builder;
    }
}