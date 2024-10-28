using Delivery.UseCases.District;
using FluentValidation;

namespace Delivery.UseCases.Orders.Queries;

public class GetDeliveryOrderQueryValidator : AbstractValidator<GetDeliveryOrderQuery>
{
    public GetDeliveryOrderQueryValidator(IDistrictRepository districtRepository)
    {
        RuleFor(x => x.DistrictId)
            .NotEmpty()
            .MustAsync(districtRepository.Exists)
            .WithMessage("District with passed id do not exist");

        RuleFor(x => x.FirstDeliveryDateTime)
            .NotNull();

        RuleFor(x => x.LastDeliveryDateTime)
            .NotNull();

        RuleFor(x => x)
            .Must(x => x.FirstDeliveryDateTime < x.LastDeliveryDateTime)
            .WithMessage("First delivery must be before last delivery");
    }
}