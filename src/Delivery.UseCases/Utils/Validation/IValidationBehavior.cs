using Delivery.UseCases.Utils.Result;
using MediatR;

namespace Delivery.UseCases.Utils.Validation;

/// <summary>
/// Command validator interface.
/// </summary>
/// <typeparam name="TRequest">Request type</typeparam>
/// <typeparam name="TResponseValue">Response type</typeparam>
public interface IValidationBehavior<in TRequest, TResponseValue> : IPipelineBehavior<TRequest, Result<TResponseValue>>
    where TRequest : IRequest<Result<TResponseValue>>
{ }