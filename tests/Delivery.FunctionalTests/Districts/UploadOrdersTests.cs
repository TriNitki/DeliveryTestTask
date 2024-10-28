using Delivery.Contracts;
using System.Net;
using Delivery.FunctionalTests.Abstractions;
using Delivery.FunctionalTests.Extensions;
using Delivery.UseCases.District.Commands.Create;
using FluentAssertions;

namespace Delivery.FunctionalTests.Districts;

public class UploadOrdersTests(FunctionalTestWebAppFactory factory) : BaseFunctionTest(factory)
{
    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsFullyValid()
    {
        var obj = new List<CreateDistrictCommand>
        {
            new("Valid"),
            new("Valid")
        };

        var response = await HttpClient.PostAsync("/api/districts/upload", ObjectToFormData(obj));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.DeserializeAsync<UploadModel<OrderModel>>();
        result.Should().NotBeNull();
        result!.UploadedModels.Should().HaveCount(2);
        result.Errors.Should().HaveCount(0);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsPartlyValid()
    {
        var obj = new List<CreateDistrictCommand>
        {
            new("Valid"),
            new(string.Empty)
        };

        var response = await HttpClient.PostAsync("/api/districts/upload", ObjectToFormData(obj));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.DeserializeAsync<UploadModel<OrderModel>>();
        result.Should().NotBeNull();
        result!.UploadedModels.Should().HaveCount(1);
        result.Errors.Should().HaveCountGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsFullyInvalid()
    {
        var obj = new List<CreateDistrictCommand>
        {
            new(string.Empty),
            new(string.Empty)
        };

        var response = await HttpClient.PostAsync("/api/districts/upload", ObjectToFormData(obj));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.DeserializeAsync<UploadModel<OrderModel>>();
        result.Should().NotBeNull();
        result!.UploadedModels.Should().HaveCount(0);
        result.Errors.Should().HaveCountGreaterOrEqualTo(2);
    }
}