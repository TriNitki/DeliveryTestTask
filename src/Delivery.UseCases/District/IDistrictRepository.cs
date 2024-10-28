namespace Delivery.UseCases.District;

/// <summary>
/// Repository for <see cref="Core.District"/>.
/// </summary>
public interface IDistrictRepository
{
    /// <summary>
    /// Checks if district exist.
    /// </summary>
    /// <param name="id">District id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see langword="true"/> if district exist, otherwise <see langword="false"/></returns>
    Task<bool> Exists(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds <see cref="Core.District"/>.
    /// </summary>
    /// <param name="district">District</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task Add(Core.District district, CancellationToken cancellationToken);
}