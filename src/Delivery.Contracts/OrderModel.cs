namespace Delivery.Contracts;

/// <summary>
/// Order model.
/// </summary>
public class OrderModel
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Weight.
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Delivery date time.
    /// </summary>
    public DateTime DateTime { get; set; }
}