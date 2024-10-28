using FluentValidation;

namespace Delivery.UseCases.District.Commands.Create;

public class CreateDistrictCommandValidator : AbstractValidator<CreateDistrictCommand>
{
    public CreateDistrictCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}