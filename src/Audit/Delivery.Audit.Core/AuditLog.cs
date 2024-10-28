namespace Delivery.Audit.Core;

/// <summary>
/// Audit log.
/// </summary>
public class AuditLog
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Creation date time.
    /// </summary>
    public DateTime DateTime { get; init; }

    /// <summary>
    /// Client ip.
    /// </summary>
    public string ClientIp { get; init; } = string.Empty;

    /// <summary>
    /// Action.
    /// </summary>
    public string Action { get; init; } = string.Empty;

    /// <summary>
    /// Whether the action was successful.
    /// </summary>
    public bool IsSuccessful { get; init; }

    /// <summary>
    /// Message.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Extra data as a json string. Default value is request body.
    /// </summary>
    public string? ExtraData { get; init; }

    /// <summary>
    /// Request id.
    /// </summary>
    public string RequestId { get; init; } = string.Empty;
}