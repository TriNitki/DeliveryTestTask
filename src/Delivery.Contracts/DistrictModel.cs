namespace Delivery.Contracts;

/// <summary>
/// District model.
/// </summary>
public class DistrictModel
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name.
    /// </summary>
    public required string Name { get; set; }
}