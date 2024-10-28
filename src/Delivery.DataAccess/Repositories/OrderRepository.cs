using Delivery.UseCases.Orders;
using Microsoft.EntityFrameworkCore;

namespace Delivery.DataAccess.Repositories;

/// <summary>
/// <see cref="IOrderRepository"/> implementation.
/// </summary>
/// <param name="context">Db context</param>
public class OrderRepository(Context context) : IOrderRepository
{
    /// <inheritdoc/>
    public IAsyncEnumerable<Core.Order> GetAll(Guid districtId, DateTime? firstDeliveryDateTime = null,
        DateTime? lastDeliveryDateTime = null)
    {
        var orders = context.Orders
            .Where(x => x.DistrictId == districtId)
            .AsQueryable().AsNoTracking();

        if (firstDeliveryDateTime is not null)
            orders = orders.Where(x => x.DateTime > firstDeliveryDateTime);

        if (lastDeliveryDateTime is not null)
            orders = orders.Where(x => x.DateTime < lastDeliveryDateTime);

        return orders.AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task Add(Core.Order order, CancellationToken cancellationToken)
    {
        await context.Orders.AddAsync(order, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}