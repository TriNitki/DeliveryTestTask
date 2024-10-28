using Delivery.UseCases.Utils.Result;
using MediatR;
using FluentValidation;

namespace Delivery.UseCases.Utils.Validation;

/// <summary>
/// <see cref="IValidationBehavior{TRequest, TResponseValue}"/> implementation.
/// </summary>
/// <typeparam name="TRequest">Request type</typeparam>
/// <typeparam name="TResponseValue">Response type</typeparam>
public class ValidationBehavior<TRequest, TResponseValue> : IValidationBehavior<TRequest, TResponseValue>
    where TRequest : IRequest<Result<TResponseValue>>
{
    private readonly IEnumerable<IValidator> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
    }

    /// <inheritdoc/>
    public async Task<Result<TResponseValue>> Handle(TRequest command, RequestHandlerDelegate<Result<TResponseValue>> next, CancellationToken cancellationToken)
    {
        if (!(_validators?.Any() ?? false)) 
            return await next();

        var context = new ValidationContext<TRequest>(command);
        var tasks = _validators.Select(x => x.ValidateAsync(context, cancellationToken));
        var validationResults = await Task.WhenAll(tasks);

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
            return Result<TResponseValue>.Invalid(failures.Select(x => x.ErrorMessage).ToArray());

        return await next();
    }
}