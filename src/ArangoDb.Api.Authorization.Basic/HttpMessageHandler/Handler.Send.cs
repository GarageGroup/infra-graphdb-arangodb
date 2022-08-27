using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.ArangoDb;

partial class ArangoDbBasicAuthDelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<HttpResponseMessage>(cancellationToken);
        }

        if (string.IsNullOrEmpty(option.Password))
        {
            return base.SendAsync(request, cancellationToken);
        }

        var basicAuthBytes = Encoding.ASCII.GetBytes($"{option.UserName}:{option.Password}");
        var basicAuthValue = Convert.ToBase64String(basicAuthBytes);

        request.Headers.Authorization = new("Basic", basicAuthValue);
        return base.SendAsync(request, cancellationToken);
    }
}