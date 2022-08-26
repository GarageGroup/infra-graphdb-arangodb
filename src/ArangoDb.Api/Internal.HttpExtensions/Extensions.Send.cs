using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.ArangoDb;

partial class HttpExtensions
{
    internal static async ValueTask<Result<Unit, DbFailureJson>> SendAsync(
        this HttpClient httpClient, HttpMethod method, string requestUri, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, requestUri);
        using var httpResponse = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        return httpResponse.IsSuccessStatusCode ? default(Unit) : await httpResponse.ReadFailureAsync(cancellationToken).ConfigureAwait(false);
    }
}