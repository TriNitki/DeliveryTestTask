namespace Delivery.Core;

/// <summary>
/// Order.
/// </summary>
public class Order
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Weight.
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Delivery date time.
    /// </summary>
    public DateTime DateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// District id.
    /// </summary>
    public Guid DistrictId { get; set; }

    /// <summary>
    /// District.
    /// </summary>
    public virtual District? District { get; set; }

    /// <summary>
    /// Delivery orders.
    /// </summary>
    public virtual List<OrderToDeliveryOrder> DeliveryOrders { get; set; } = [];
}