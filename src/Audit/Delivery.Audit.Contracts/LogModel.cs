namespace Delivery.Audit.Contracts;

/// <summary>
/// Log model.
/// </summary>
/// <param name="action">Action</param>
/// <param name="isSuccess">Whether the action was successful</param>
/// <param name="message">Message</param>
public class LogModel(string action, bool isSuccess, string message)
{
    /// <summary>
    /// Action.
    /// </summary>
    public string Action { get; set; } = action;

    /// <summary>
    /// Message.
    /// </summary>
    public string Message { get; set; } = message;

    /// <summary>
    /// Whether the action was successful.
    /// </summary>
    public bool IsSuccess { get; set; } = isSuccess;
}