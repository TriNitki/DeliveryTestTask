using NLog;
using NLog.Targets;

namespace Delivery.Logger;

internal class AuditTarget : AsyncTaskTarget
{
    protected override Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
