namespace Delivery.Logger;

internal interface IAuditLogger
{
    public void Log(string action, bool isSuccessful, string message);
}