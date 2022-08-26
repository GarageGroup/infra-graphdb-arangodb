using System;

namespace GGroupp.Infra.ArangoDb;

public sealed record class ArangoDbApiOption
{
    public ArangoDbApiOption(Uri baseAddress, string databaseId, string jwt)
    {
        BaseAddress = baseAddress;
        DatabaseId = databaseId ?? string.Empty;
        Jwt = jwt ?? string.Empty;
    }

    public Uri BaseAddress { get; }

    public string DatabaseId { get; }

    public string Jwt { get; }
}