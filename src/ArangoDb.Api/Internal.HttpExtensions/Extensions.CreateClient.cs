using System.Net.Http;

namespace GGroupp.Infra.ArangoDb;

partial class HttpExtensions
{
    internal static HttpClient CreateHttpClient(this HttpMessageHandler httpMessageHandler, ArangoDbApiOption option)
    {
        var dbName = string.IsNullOrEmpty(option.DatabaseId) ? "_system" : option.DatabaseId;

        var httpClient = new HttpClient(httpMessageHandler, false)
        {
            BaseAddress = new(option.BaseAddress, "_db/" + dbName + "/")
        };

        if (string.IsNullOrEmpty(option.Jwt) is false)
        {
            httpClient.DefaultRequestHeaders.Authorization = new("bearer", option.Jwt);
        }

        return httpClient;
    }
}