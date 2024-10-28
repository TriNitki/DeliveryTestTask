using Delivery.FunctionalTests.Abstractions;
using Delivery.UseCases.District.Commands.Create;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace Delivery.FunctionalTests.Districts;

public class CreateDistrictTests(FunctionalTestWebAppFactory factory) : BaseFunctionTest(factory)
{
    [Fact]
    public async Task Should_ReturnCreated_WhenRequestIsValid()
    {
        var request = new CreateDistrictCommand("asdasd");
        var response = await HttpClient.PostAsJsonAsync("api/district", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}