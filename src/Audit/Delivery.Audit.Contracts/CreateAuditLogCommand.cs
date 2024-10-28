using MediatR;

namespace Delivery.Audit.Contracts;

public class CreateAuditLogCommand : IRequest<Unit>
{
    public DateTime DateTime { get; init; }

    public string ClientIp { get; init; } = string.Empty;

    public string Action { get; init; } = string.Empty;

    public bool IsSuccessful { get; init; }

    public string Message { get; init; } = string.Empty;

    public string? ExtraData { get; init; }

    public string RequestId { get; init; } = string.Empty;
}