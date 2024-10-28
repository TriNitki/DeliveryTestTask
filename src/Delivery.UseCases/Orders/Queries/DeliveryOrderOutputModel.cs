using Delivery.Contracts;

namespace Delivery.UseCases.Orders.Queries;

public class DeliveryOrderOutputModel : DeliveryOrderModel
{
    public List<OrderModel> Orders { get; set; } = [];
}