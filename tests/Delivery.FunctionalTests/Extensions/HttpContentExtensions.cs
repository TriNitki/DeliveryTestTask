using Newtonsoft.Json;

namespace Delivery.FunctionalTests.Extensions;

internal static class HttpContentExtensions
{
    internal static async Task<T?> DeserializeAsync<T>(this HttpContent content)
    {
        return JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
    }
}