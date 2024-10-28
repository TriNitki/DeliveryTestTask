using Delivery.Audit.Contracts;
using Delivery.UseCases.Utils.Result;

namespace Delivery.Service.Infrastructure;

/// <summary>
/// Extensions for <see cref="Result{TResponseValue}"/>.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts <see cref="Result{TResponseValue}"/> to <see cref="LogModel"/>.
    /// </summary>
    /// <typeparam name="TResponseValue">Response</typeparam>
    /// <param name="result">Result</param>
    /// <param name="action">Action</param>
    /// <returns>Log model</returns>
    public static LogModel ToLog<TResponseValue>(this in Result<TResponseValue> result, string action)
    {
        return new LogModel(action, result.IsSuccess, string.Join(". ", result.Errors ?? []));
    }
}