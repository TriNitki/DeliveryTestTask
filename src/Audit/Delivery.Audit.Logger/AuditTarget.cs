using Delivery.Audit.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Targets;

namespace Delivery.Audit.Logger;

/// <summary>
/// Custom target for audit logging.
/// </summary>
public sealed class AuditTarget : AsyncTaskTarget
{
    private readonly IMediator _mediator;

    public AuditTarget(IServiceScopeFactory serviceScopeFactory)
    {
        _mediator = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMediator>()
                    ?? throw new ArgumentNullException(null, "IMediator service was not retrieved from the DI container.");
        IncludeEventProperties = true;

        ContextProperties.Add(new TargetPropertyWithContext("client_ip", "${aspnet-request-ip}"));
        ContextProperties.Add(new TargetPropertyWithContext("request_id", "${aspnet-traceidentifier}"));
        ContextProperties.Add(new TargetPropertyWithContext("posted_body", "${aspnet-request-posted-body}"));
    }

    /// <inheritdoc/>
    protected override Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
    {
        var props = GetAllProperties(logEvent);
        if (!bool.TryParse(GetLogProperty(props, "is_successful")!, out var isSuccessful))
            isSuccessful = true;
        
        return _mediator.Send(new CreateAuditLogCommand
        {
            DateTime = logEvent.TimeStamp.ToUniversalTime(),
            Message = logEvent.Message,
            Action = GetLogProperty(props, "action") ?? "Internal",
            IsSuccessful = isSuccessful,
            ClientIp = GetLogProperty(props, "client_ip")!,
            RequestId = GetLogProperty(props, "request_id")!,
            ExtraData = GetLogProperty(props, "posted_body")
        }, cancellationToken);
    }

    /// <summary>
    /// Gets log property by name.
    /// </summary>
    /// <param name="props">Properties</param>
    /// <param name="propName">Property name</param>
    /// <returns>Property</returns>
    private static string? GetLogProperty(IDictionary<string, object> props, string propName)
    {
        return props.TryGetValue(propName, out var prop) ? prop.ToString() : null;
    }
}
