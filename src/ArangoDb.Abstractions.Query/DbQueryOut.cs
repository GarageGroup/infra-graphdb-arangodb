using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra.ArangoDb;

public sealed record class DbQueryOut<T>
{
    public DbQueryOut(
        [AllowNull] string transactionId,
        [AllowNull] string cursorId,
        [AllowNull] IReadOnlyCollection<T> documents,
        bool cached,
        long count,
        bool hasMore)
    {
        TransactionId = string.IsNullOrEmpty(transactionId) ? null : transactionId;
        CursorId = string.IsNullOrEmpty(cursorId) ? null : cursorId;
        Documents = documents ?? Array.Empty<T>();
        Cached = cached;
        Count = count;
        HasMore = hasMore;
    }

    public string? TransactionId { get; }

    public string? CursorId { get; }

    public IReadOnlyCollection<T> Documents { get; }

    public bool Cached { get; }

    public long Count { get; }

    public bool HasMore { get; }
}