using System.Collections.Generic;

namespace GGroupp.Infra.ArangoDb;

internal readonly record struct DbCursorJsonOut<T>
{
    public long Count { get; init; }

    public bool Cached { get; init; }

    public bool HasMore { get; init; }

    public IReadOnlyCollection<T>? Result { get; init; }

    public string? Id { get; init; }
}