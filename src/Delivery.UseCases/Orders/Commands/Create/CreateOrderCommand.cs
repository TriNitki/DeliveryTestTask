using AutoMapper;
using Delivery.Contracts;
using Delivery.Core;
using Delivery.UseCases.Utils.Result;
using Delivery.UseCases.Utils.Validation;
using MediatR;

namespace Delivery.UseCases.Orders.Commands.Create;

public class CreateOrderCommand(double weight, Guid districtId, DateTime dateTime) : IValidatableCommand<OrderModel>
{
    public double Weight { get; set; } = weight;

    public DateTime DateTime { get; set; } = dateTime.ToUniversalTime();

    public Guid DistrictId { get; set; } = districtId;
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderModel>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<OrderModel>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Weight = request.Weight,
            DateTime = request.DateTime,
            DistrictId = request.DistrictId
        };
        await _orderRepository.Add(order, cancellationToken);
        return Result<OrderModel>.Created(_mapper.Map<OrderModel>(order));
    }
}