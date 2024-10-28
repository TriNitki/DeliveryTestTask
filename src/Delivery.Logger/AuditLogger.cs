using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Targets;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Delivery.Logger;

internal class AuditLogger : IAuditLogger
{
    /// <summary>
    /// Название сервиса
    /// </summary>
    protected string _serviveName;

    /// <summary>
    /// Медиатр.
    /// </summary>
    private IMediator _mediator;

    /// <inheritdoc/>
    protected override Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
    {
        var props = GetAllProperties(logEvent);

        return _mediator.Send(new CreateAuditLogCommand()
        {
            DateTime = logEvent.TimeStamp.ToUniversalTime(),
            ServiceName = _serviveName,
            Message = logEvent.Message,
            Action = GetLogProperty(props, "action")!,
            IsSuccessful = bool.Parse(GetLogProperty(props, "is_successful")!),
            ClientId = GetLogProperty(props, "client_id")!,
            ClientIp = GetLogProperty(props, "client_ip")!,
            RequestId = GetLogProperty(props, "request_id")!,
            ExtraData = GetLogProperty(props, "posted_body")
        }, cancellationToken);
    }

    public virtual void Build(IMediator mediator)
    {
        IncludeEventProperties = true;

        ContextProperties.Add(new TargetPropertyWithContext("client_ip", "${aspnet-request-ip}"));
        ContextProperties.Add(new TargetPropertyWithContext("client_id", "${aspnet-user-claim:ClaimTypes.NameIdentifier}"));
        ContextProperties.Add(new TargetPropertyWithContext("request_id", "${aspnet-traceidentifier}"));
        ContextProperties.Add(new TargetPropertyWithContext("posted_body", "${aspnet-request-posted-body}"));

        _mediator = ServiceProvider.GetRequiredService<IMediator>()
                    ?? throw new ArgumentNullException(null, "Сервис IMediator не был получен из DI контейнера.");
    }

    /// <summary>
    /// Получение свойства из списка.
    /// </summary>
    /// <param name="props"> Список свойств. </param>
    /// <param name="propName"> Название свойства. </param>
    /// <returns> Свойство. </returns>
    protected string? GetLogProperty(IDictionary<string, object> props, string propName)
    {
        return props.TryGetValue(propName, out var prop) ? prop.ToString() : null;
    }
}