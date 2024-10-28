using Delivery.Audit.Core;

namespace Delivery.Audit.UseCases.Abstractions;

/// <summary>
/// Repository for <see cref="AuditLog"/>.
/// </summary>
public interface IAuditLogRepository
{
    /// <summary>
    /// Adds <see cref="AuditLog"/>.
    /// </summary>
    /// <param name="auditLog"> Audit log </param>
    Task Add(AuditLog auditLog);
}