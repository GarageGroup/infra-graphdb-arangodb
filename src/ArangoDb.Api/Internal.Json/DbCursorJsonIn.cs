using System.Collections.Generic;

namespace GGroupp.Infra.ArangoDb;

internal readonly record struct DbCursorJsonIn
{
    public string? Query { get; init; }

    public IReadOnlyDictionary<string, object>? BindVars { get; init; }

    public DbCursorOptionJson? Options { get; init; }

    public bool? Count { get; init; }

    public long? BatchSize { get; init; }

    public bool? Cache { get; init; }

    public int? Ttl { get; init; }
}