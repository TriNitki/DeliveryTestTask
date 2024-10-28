using Delivery.Audit.UseCases.Abstractions;

namespace Delivery.DataAccess.Repositories;

/// <summary>
/// <see cref="IAuditLogRepository"/> implementation.
/// </summary>
/// <param name="context">Db context</param>
public class AuditLogRepository(Context context) : IAuditLogRepository
{
    /// <inheritdoc/>
    public async Task Add(Audit.Core.AuditLog auditLog)
    {
        await context.AddAsync(auditLog);
        await context.SaveChangesAsync();
    }
}