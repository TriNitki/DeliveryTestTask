using Delivery.UseCases.Utils.Result;
using MediatR;

namespace Delivery.UseCases.Utils.Validation;

/// <summary>
/// Command with validation
/// </summary>
/// <typeparam name="TResponseValue">Response type</typeparam>
public interface IValidatableCommand<TResponseValue> : IRequest<Result<TResponseValue>>
{ }