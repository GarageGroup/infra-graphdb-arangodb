using System;
using System.Net.Http;

namespace GGroupp.Infra.ArangoDb;

internal sealed partial class ArangoDbBasicAuthDelegatingHandler : DelegatingHandler
{
    public static ArangoDbBasicAuthDelegatingHandler Create(HttpMessageHandler innerHandler, ArangoDbBasicAuthOption option)
        =>
        new(
            innerHandler ?? throw new ArgumentNullException(nameof(innerHandler)),
            option ?? throw new ArgumentNullException(nameof(option)));

    private readonly ArangoDbBasicAuthOption option;

    private ArangoDbBasicAuthDelegatingHandler(HttpMessageHandler innerHandler, ArangoDbBasicAuthOption option)
        : base(innerHandler)
        =>
        this.option = option;
}