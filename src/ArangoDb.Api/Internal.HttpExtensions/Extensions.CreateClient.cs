using System.Net.Http;

namespace GGroupp.Infra.ArangoDb;

partial class HttpExtensions
{
    internal static HttpClient CreateHttpClient(this HttpMessageHandler httpMessageHandler, ArangoDbApiOption option)
    {
        var dbName = string.IsNullOrEmpty(option.DatabaseId) ? "_system" : option.DatabaseId;
        return new(httpMessageHandler, false)
        {
            BaseAddress = new(option.BaseAddress, "_db/" + dbName + "/")
        };
    }
}