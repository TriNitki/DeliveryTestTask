namespace Delivery.Core;

/// <summary>
/// District
/// </summary>
public class District
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Name.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Delivery orders.
    /// </summary>
    public virtual List<DeliveryOrder> DeliveryOrders { get; set; } = [];
}