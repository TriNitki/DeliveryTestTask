using System.Net;
using System.Net.Http.Json;
using Delivery.FunctionalTests.Abstractions;
using Delivery.UseCases.Orders.Commands.Create;
using FluentAssertions;

namespace Delivery.FunctionalTests.Orders;

public class CreateOrderTests(FunctionalTestWebAppFactory factory) : BaseOrderTest(factory)
{
    [Fact]
    public async Task Should_ReturnBadRequest_WhenRequestIsInvalid()
    {
        var request = new CreateOrderCommand(-1, Guid.NewGuid(), DateTime.UtcNow.AddHours(1));

        var response = await HttpClient.PostAsJsonAsync("api/order", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnCreated_WhenRequestIsValid()
    {
        var request = new CreateOrderCommand(1, District.Id, DateTime.UtcNow.AddHours(1));

        var response = await HttpClient.PostAsJsonAsync("api/order", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}