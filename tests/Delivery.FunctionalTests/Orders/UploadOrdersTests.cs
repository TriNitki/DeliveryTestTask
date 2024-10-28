using Delivery.FunctionalTests.Abstractions;
using Delivery.UseCases.Orders.Commands.Create;
using FluentAssertions;
using System.Net;
using Delivery.Contracts;
using Delivery.FunctionalTests.Extensions;

namespace Delivery.FunctionalTests.Orders;

public class UploadOrdersTests(FunctionalTestWebAppFactory factory) : BaseOrderTest(factory)
{
    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsFullyValid()
    {
        var obj = new List<CreateOrderCommand>
        {
            new(1.0, District.Id, DateTime.UtcNow.AddHours(1)),
            new(2.0, District.Id, DateTime.UtcNow.AddMinutes(25))
        };

        var response = await HttpClient.PostAsync("/api/orders/upload", ObjectToFormData(obj));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.DeserializeAsync<UploadModel<OrderModel>>();
        result.Should().NotBeNull();
        result!.UploadedModels.Should().HaveCount(2);
        result.Errors.Should().HaveCount(0);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsPartlyValid()
    {
        var obj = new List<CreateOrderCommand>
        {
            new(1.0, District.Id, DateTime.UtcNow.AddHours(1)),
            new(-2.0, District.Id, DateTime.UtcNow.AddMinutes(25))
        };

        var response = await HttpClient.PostAsync("/api/orders/upload", ObjectToFormData(obj));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.DeserializeAsync<UploadModel<OrderModel>>();
        result.Should().NotBeNull();
        result!.UploadedModels.Should().HaveCount(1);
        result.Errors.Should().HaveCountGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsFullyInvalid()
    {
        var obj = new List<CreateOrderCommand>
        {
            new(1.0, District.Id, DateTime.UtcNow.Subtract(TimeSpan.FromHours(1))),
            new(-2.0, District.Id, DateTime.UtcNow.AddMinutes(25))
        };

        var response = await HttpClient.PostAsync("/api/orders/upload", ObjectToFormData(obj));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.DeserializeAsync<UploadModel<OrderModel>>();
        result.Should().NotBeNull();
        result!.UploadedModels.Should().HaveCount(0);
        result.Errors.Should().HaveCountGreaterOrEqualTo(2);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenObjectIsInvalid()
    {
        var response = await HttpClient.PostAsync("/api/orders/upload", ObjectToFormData(1));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}