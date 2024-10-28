using Delivery.Core;

namespace Delivery.UseCases.Orders;

/// <summary>
/// Repository for <see cref="Order"/>.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Gets orders.
    /// </summary>
    /// <param name="districtId">District id</param>
    /// <param name="firstDeliveryDateTime">First delivery date time</param>
    /// <param name="lastDeliveryDateTime">Last delivery date time</param>
    /// <returns>Orders</returns>
    IAsyncEnumerable<Order> GetAll(Guid districtId, DateTime? firstDeliveryDateTime = null, DateTime? lastDeliveryDateTime = null);

    /// <summary>
    /// Add <see cref="Order"/>.
    /// </summary>
    /// <param name="order">Order</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task Add(Order order, CancellationToken cancellationToken);
}