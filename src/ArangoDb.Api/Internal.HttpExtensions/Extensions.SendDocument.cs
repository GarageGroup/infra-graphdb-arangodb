using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.ArangoDb;

partial class HttpExtensions
{
    internal static async ValueTask<Result<TOut, DbFailureJson>> SendDocumentAsync<TIn, TOut>(
        this HttpClient httpClient, HttpMethod method, string requestUri, TIn? document, CancellationToken cancellationToken)
        where TOut : struct
    {
        using var request = new HttpRequestMessage(method, requestUri)
        {
            Content = CreateDocumentContent(document)
        };

        using var httpResponse = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (httpResponse.IsSuccessStatusCode is false)
        {
            return await httpResponse.ReadFailureAsync(cancellationToken).ConfigureAwait(false);
        }

        return await httpResponse.ReadContentAsync<TOut>(cancellationToken).ConfigureAwait(false);
    }
}