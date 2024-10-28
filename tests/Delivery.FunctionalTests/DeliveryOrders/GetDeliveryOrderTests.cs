using System.Net;
using Delivery.FunctionalTests.Abstractions;
using Delivery.FunctionalTests.Extensions;
using Delivery.UseCases.Orders.Queries;
using FluentAssertions;

namespace Delivery.FunctionalTests.DeliveryOrders;

public class GetDeliveryOrderTests(FunctionalTestWebAppFactory factory) : BaseDeliveryOrderTest(factory)
{
    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        var uri = GetUri(District.Id);
        var response = await HttpClient.GetAsync(uri);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.DeserializeAsync<DeliveryOrderOutputModel>();

        result!.Orders.Should().HaveCount(2);

        result.Orders.Should()
            .ContainEquivalentOf(Orders[0], 
            options => options.Excluding(x => x.DateTime))
            .And
            .ContainEquivalentOf(Orders[1],
                options => options.Excluding(x => x.DateTime))
            .And
            .NotContainEquivalentOf(Orders[2],
                options => options.Excluding(x => x.DateTime));
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenDistrictIdIsInvalid()
    {
        var uri = GetUri(Guid.Empty);
        var response = await HttpClient.GetAsync(uri);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenDateTimesAreInvalid()
    {
        var uri = GetUri(District.Id, DateTime.UtcNow.AddMinutes(30), DateTime.UtcNow);
        var response = await HttpClient.GetAsync(uri);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}