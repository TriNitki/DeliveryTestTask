using Delivery.Contracts;
using Delivery.FunctionalTests.Abstractions;
using Delivery.FunctionalTests.Orders;
using System.Net.Http.Json;
using Delivery.FunctionalTests.Extensions;
using Delivery.UseCases.Orders.Commands.Create;
using System.Web;

namespace Delivery.FunctionalTests.DeliveryOrders;

public abstract class BaseDeliveryOrderTest : BaseOrderTest
{
    public readonly List<OrderModel> Orders;

    protected BaseDeliveryOrderTest(FunctionalTestWebAppFactory factory) : base(factory)
    {
        Orders = CreateOrders().Result;
    }

    private async Task<List<OrderModel>> CreateOrders()
    {
        var request1 = new CreateOrderCommand(1, District.Id, DateTime.UtcNow.AddMinutes(10));
        var request2 = new CreateOrderCommand(2, District.Id, DateTime.UtcNow.AddMinutes(20));
        var request3 = new CreateOrderCommand(3, District.Id, DateTime.UtcNow.AddMinutes(40));

        var result1 = await HttpClient.PostAsJsonAsync("api/order", request1);
        var result2 = await HttpClient.PostAsJsonAsync("api/order", request2);
        var result3 = await HttpClient.PostAsJsonAsync("api/order", request3);

        List<OrderModel> orders = [
            result1.Content.DeserializeAsync<OrderModel>().Result!,
            result2.Content.DeserializeAsync<OrderModel>().Result!,
            result3.Content.DeserializeAsync<OrderModel>().Result!
        ];
        return orders;
    }

    protected static string GetUri(
        Guid districtId, DateTime? firstDeliveryDateTime = null, DateTime? lastDeliveryDateTime = null, string baseUri = "localhost/api/orders")
    {
        var uriBuilder = new UriBuilder(baseUri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["districtId"] = districtId.ToString();

        if(firstDeliveryDateTime is not null)
            query["firstDeliveryDateTime"] = firstDeliveryDateTime.ToString();

        if(lastDeliveryDateTime is not null)
            query["lastDeliveryDateTime"] = lastDeliveryDateTime.ToString();

        uriBuilder.Query = query.ToString();
        return uriBuilder.ToString();
    }
}