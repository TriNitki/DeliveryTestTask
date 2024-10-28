namespace Delivery.Core;

/// <summary>
/// Delivery order.
/// </summary>
public class DeliveryOrder
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Creation date time.
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
    /// Orders.
    /// </summary>
    public virtual List<OrderToDeliveryOrder> Orders { get; set; } = [];
}