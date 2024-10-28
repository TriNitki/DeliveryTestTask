using AutoMapper;
using Delivery.Contracts;
using Delivery.UseCases.Utils.Result;
using Delivery.UseCases.Utils.Validation;
using MediatR;

namespace Delivery.UseCases.District.Commands.Create;

public class CreateDistrictCommand(string name) : IValidatableCommand<DistrictModel>
{
    public string Name { get; set; } = name;
}

public class CreateDistrictCommandHandler : IRequestHandler<CreateDistrictCommand, Result<DistrictModel>>
{
    private readonly IDistrictRepository _districtRepository;
    private readonly IMapper _mapper;

    public CreateDistrictCommandHandler(IDistrictRepository districtRepository, IMapper mapper)
    {
        _districtRepository = districtRepository ?? throw new ArgumentNullException(nameof(districtRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<DistrictModel>> Handle(CreateDistrictCommand request, CancellationToken cancellationToken)
    {
        var district = new Core.District { Name = request.Name };
        await _districtRepository.Add(district, cancellationToken);
        return Result<DistrictModel>.Created(_mapper.Map<DistrictModel>(district));
    }
}