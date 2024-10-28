using Delivery.Audit.Contracts;

namespace Delivery.Audit.Logger;

/// <summary>
/// Audit logger.
/// </summary>
public interface IAuditLogger
{
    /// <summary>
    /// Create log.
    /// </summary>
    /// <param name="action">Action</param>
    /// <param name="isSuccessful">Whether the action is successful</param>
    /// <param name="message">Message</param>
    public void Log(string action, bool isSuccessful, string message);

    /// <summary>
    /// Create log.
    /// </summary>
    /// <param name="log">Log model</param>
    public void Log(LogModel log);
}