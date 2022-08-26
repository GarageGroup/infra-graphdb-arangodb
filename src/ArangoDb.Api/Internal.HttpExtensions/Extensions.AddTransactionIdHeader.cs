using System.Net.Http;

namespace GGroupp.Infra.ArangoDb;

partial class HttpExtensions
{
    internal static HttpClient AddTransactionIdHeader(this HttpClient httpClient, string? transactionId)
    {
        if (string.IsNullOrEmpty(transactionId) is false)
        {
            httpClient.DefaultRequestHeaders.Add(TransactionIdHeaderName, transactionId);
        }

        return httpClient;
    }
}