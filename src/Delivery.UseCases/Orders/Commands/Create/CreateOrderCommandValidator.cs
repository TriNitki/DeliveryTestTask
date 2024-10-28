using Delivery.UseCases.District;
using FluentValidation;

namespace Delivery.UseCases.Orders.Commands.Create;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(IDistrictRepository districtRepository)
    {
        RuleFor(x => x.DistrictId)
            .NotEmpty()
            .MustAsync(districtRepository.Exists)
            .WithMessage(x => $"District with passed id do not existValue: '{x.DistrictId}'");

        RuleFor(x => x.Weight)
            .Must(x => x >= 0)
            .WithMessage(x => $"Weight must be equal to or greater than 0. Value: '{x.Weight}'");

        RuleFor(x => x.DateTime)
            .Must(x => x > DateTime.UtcNow)
            .WithMessage(x => $"Not possible to create orders in the past. Value: '{x.DateTime}'");
    }
}