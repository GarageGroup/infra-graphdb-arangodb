using System.Net;

namespace GGroupp.Infra.ArangoDb;

internal readonly record struct DbFailureJson
{
    public string? ErrorMessage { get; init; }

    public int ErrorNum { get; init; }

    public HttpStatusCode Code { get; init; }
}