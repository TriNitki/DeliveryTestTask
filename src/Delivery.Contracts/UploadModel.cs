namespace Delivery.Contracts;

/// <summary>
/// Upload model.
/// </summary>
/// <typeparam name="T">Uploaded models</typeparam>
public class UploadModel<T>
{
    /// <summary>
    /// Uploaded model.
    /// </summary>
    public List<T> UploadedModels { get; set; } = [];

    /// <summary>
    /// List of errors.
    /// </summary>
    public List<string> Errors { get; set; } = [];
}