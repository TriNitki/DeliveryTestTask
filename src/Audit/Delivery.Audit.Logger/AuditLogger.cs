using Delivery.Audit.Contracts;
using NLog;
using NLog.Targets;

namespace Delivery.Audit.Logger;

/// <summary>
/// <see cref="IAuditLogger"/> implementation.
/// </summary>
/// <param name="target">Audit target</param>
public class AuditLogger(AuditTarget target) : IAuditLogger
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly NLog.Logger _logger = CreateLogger(target);

    /// <inheritdoc/>
    public void Log(string action, bool isSuccessful, string message)
    {
        var logEvent = _logger.ForLogEvent(LogLevel.Info).LogEvent!;
        logEvent.Properties["action"] = action;
        logEvent.Properties["is_successful"] = isSuccessful;
        logEvent.Message = message;
        _logger.Log(logEvent);
    }

    /// <inheritdoc/>
    public void Log(LogModel log) 
        => Log(log.Action, log.IsSuccess, log.Message);

    /// <summary>
    /// Creates logger.
    /// </summary>
    /// <param name="target">Target</param>
    /// <returns>Logger.</returns>
    private static NLog.Logger CreateLogger(AsyncTaskTarget target)
    {
        var logger = LogManager.GetLogger("AuditLogger");
        var config = logger.Factory.Configuration;
        config.AddTarget("audit", target);
        config.AddRule(LogLevel.Info, LogLevel.Info, target);
        logger.Factory.Configuration = config;

        return logger;
    }
}