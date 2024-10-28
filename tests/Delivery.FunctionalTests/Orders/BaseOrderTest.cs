using Delivery.Contracts;
using Delivery.FunctionalTests.Abstractions;
using Delivery.UseCases.District.Commands.Create;
using System.Net.Http.Json;
using Delivery.FunctionalTests.Extensions;

namespace Delivery.FunctionalTests.Orders;

public abstract class BaseOrderTest : BaseFunctionTest
{
    protected readonly DistrictModel District;

    protected BaseOrderTest(FunctionalTestWebAppFactory factory) : base(factory)
    {
        District = CreateDistrict().Result;
    }

    private async Task<DistrictModel> CreateDistrict()
    {
        var request = new CreateDistrictCommand(Guid.NewGuid().ToString());
        var result = await HttpClient.PostAsJsonAsync("api/district", request);
        var district = await result.Content.DeserializeAsync<DistrictModel>();
        return district!;
    }
}