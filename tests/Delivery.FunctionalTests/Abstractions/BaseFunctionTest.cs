using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace Delivery.FunctionalTests.Abstractions;

public abstract class BaseFunctionTest : IClassFixture<FunctionalTestWebAppFactory>
{
    protected BaseFunctionTest(FunctionalTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }

    protected HttpClient HttpClient { get; init; }

    protected static MultipartFormDataContent ObjectToFormData(object obj)
    {
        var jsonString = JsonConvert.SerializeObject(obj);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

        var formFile = new FormFile(stream, 0, stream.Length, "file", "sample")
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/json"
        };

        var content = new MultipartFormDataContent
        {
            { new StreamContent(formFile.OpenReadStream()), "file", formFile.FileName }
        };
        return content;
    }
}