using AutoMapper;
using Delivery.Contracts;
using Delivery.Core;
using Delivery.UseCases.Orders.Queries;

namespace Delivery.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.District, DistrictModel>();

        CreateMap<Order, OrderModel>();

        CreateMap<DeliveryOrder, DeliveryOrderOutputModel>()
            .ForMember(model => model.Orders, opts => opts.MapFrom(
                    delivery => delivery.Orders.Select(x => new OrderModel
                    {
                        DateTime = x.Order.DateTime,
                        Weight = x.Order.Weight,
                        Id = x.Order.Id
                    })));
    }
}