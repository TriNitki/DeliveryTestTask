namespace Delivery.Contracts;

/// <summary>
/// Delivery order model.
/// </summary>
public class DeliveryOrderModel
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Creation date time.
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// District.
    /// </summary>
    public DistrictModel? District { get; set; }
}