using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra.ArangoDb;

public sealed record class DbQueryIn
{
    private static readonly IReadOnlyDictionary<string, object?> EmptyQueryParameters;

    static DbQueryIn()
        =>
        EmptyQueryParameters = new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>());

    public DbQueryIn(string query, [AllowNull] IReadOnlyDictionary<string, object?> queryParameters)
    {
        Query = query ?? string.Empty;
        QueryParameters = queryParameters ?? EmptyQueryParameters;
    }

    public string? TransactionId { get; init; }

    public string? CursorId { get; init; }

    public string Query { get; }

    public IReadOnlyDictionary<string, object?> QueryParameters { get; }

    public long? BatchSize { get; init; }

    public bool? Cache { get; init; }

    public bool? Count { get; init; }

    public bool? Stream { get; init; }

    public TimeSpan? CursorTtl { get; init; }
}