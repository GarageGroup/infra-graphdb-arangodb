using System;

namespace GGroupp.Infra.ArangoDb;

public sealed record class ArangoDbApiOption
{
    public ArangoDbApiOption(Uri baseAddress, string databaseId)
    {
        BaseAddress = baseAddress;
        DatabaseId = databaseId ?? string.Empty;
    }

    public Uri BaseAddress { get; }

    public string DatabaseId { get; }
}