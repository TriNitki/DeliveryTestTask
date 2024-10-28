using Delivery.Core;
using Delivery.UseCases.District;
using Microsoft.EntityFrameworkCore;

namespace Delivery.DataAccess.Repositories;

/// <summary>
/// <see cref="IDistrictRepository"/> implementation.
/// </summary>
/// <param name="context">Db context</param>
public class DistrictRepository(Context context) : IDistrictRepository
{
    /// <inheritdoc/>
    public Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return context.Districts.AnyAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Add(District district, CancellationToken cancellationToken)
    {
        await context.Districts.AddAsync(district, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}