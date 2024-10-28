using Delivery.Core;
using Delivery.UseCases.Orders;
using Microsoft.EntityFrameworkCore;

namespace Delivery.DataAccess.Repositories;

/// <summary>
/// <see cref="IDeliveryOrderRepository"/> implementation.
/// </summary>
/// <param name="context">Db context</param>
public class DeliveryOrderRepository(Context context) : IDeliveryOrderRepository
{
    /// <inheritdoc/>
    public async Task Add(DeliveryOrder deliveryOrder, CancellationToken cancellationToken)
    {
        await context.OrderToDeliveryOrders.AddRangeAsync(deliveryOrder.Orders, cancellationToken);
        deliveryOrder.Orders = [];
        await context.DeliveryOrders.AddAsync(deliveryOrder, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<DeliveryOrder?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return context.DeliveryOrders.AsNoTrackingWithIdentityResolution()
            .Include(x => x.Orders)
                .ThenInclude(x => x.Order)
            .Include(x => x.District)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}