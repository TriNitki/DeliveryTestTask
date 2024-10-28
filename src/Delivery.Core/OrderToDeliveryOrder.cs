namespace Delivery.Core;

/// <summary>
/// Order to delivery order.
/// </summary>
public class OrderToDeliveryOrder
{
    /// <summary>
    /// Order id.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Order.
    /// </summary>
    public virtual Order? Order { get; set; }

    /// <summary>
    /// Delivery order id.
    /// </summary>
    public Guid DeliveryOrderId { get; set; }

    /// <summary>
    /// Delivery order.
    /// </summary>
    public virtual DeliveryOrder? DeliveryOrder { get; set; }
}