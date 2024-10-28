using Delivery.Core;

namespace Delivery.UseCases.Orders;

/// <summary>
/// Repository for <see cref="DeliveryOrder"/>.
/// </summary>
public interface IDeliveryOrderRepository
{
    /// <summary>
    /// Add <see cref="DeliveryOrder"/>.
    /// </summary>
    /// <param name="deliveryOrder">Delivery order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task Add(DeliveryOrder deliveryOrder, CancellationToken cancellationToken);

    /// <summary>
    /// Gets <see cref="DeliveryOrder"/> by id.
    /// </summary>
    /// <param name="id">Delivery id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Delivery order</returns>
    Task<DeliveryOrder?> GetById(Guid id, CancellationToken cancellationToken);
}