using AutoMapper;
using Delivery.Core;
using Delivery.UseCases.Utils.Result;
using Delivery.UseCases.Utils.Validation;
using MediatR;

namespace Delivery.UseCases.Orders.Queries;

public class GetDeliveryOrderQuery : IValidatableCommand<DeliveryOrderOutputModel>
{
    public Guid DistrictId { get; set; }

    public DateTime FirstDeliveryDateTime { get; set; }

    public DateTime LastDeliveryDateTime { get; set; }

    public GetDeliveryOrderQuery(Guid districtId, DateTime? firstDeliveryDateTime = null, DateTime? lastDeliveryDateTime = null)
    {
        DistrictId = districtId;
        FirstDeliveryDateTime = firstDeliveryDateTime ?? DateTime.UtcNow;
        LastDeliveryDateTime = lastDeliveryDateTime ?? FirstDeliveryDateTime.AddMinutes(30);

        FirstDeliveryDateTime = FirstDeliveryDateTime.ToUniversalTime();
        LastDeliveryDateTime = LastDeliveryDateTime.ToUniversalTime();
    }
}

public class GetDeliveryOrderQueryHandler : IRequestHandler<GetDeliveryOrderQuery, Result<DeliveryOrderOutputModel>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryOrderRepository _deliveryOrderRepository;
    private readonly IMapper _mapper;

    public GetDeliveryOrderQueryHandler(
        IOrderRepository orderRepository, IDeliveryOrderRepository deliveryOrderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _deliveryOrderRepository = deliveryOrderRepository ?? throw new ArgumentNullException(nameof(deliveryOrderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<DeliveryOrderOutputModel>> Handle(GetDeliveryOrderQuery request, CancellationToken cancellationToken)
    {
        var orders = _orderRepository.GetAll(
            request.DistrictId, request.FirstDeliveryDateTime, request.LastDeliveryDateTime);

        List<Order> orderList = [];
        await foreach (var order in orders)
            orderList.Add(order);

        var deliveryOrder = new DeliveryOrder { DistrictId = request.DistrictId };
        deliveryOrder.Orders = orderList.Select(x => new OrderToDeliveryOrder
        {
            DeliveryOrderId = deliveryOrder.Id,
            OrderId = x.Id
        }).ToList();
        await _deliveryOrderRepository.Add(deliveryOrder, cancellationToken);
        var deliveryOrderDto = await _deliveryOrderRepository.GetById(deliveryOrder.Id, cancellationToken);
        return Result<DeliveryOrderOutputModel>.Success(_mapper.Map<DeliveryOrderOutputModel>(deliveryOrderDto));
    }
}