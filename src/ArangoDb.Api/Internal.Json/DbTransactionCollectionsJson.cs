using System.Collections.Generic;

namespace GGroupp.Infra.ArangoDb;

internal readonly record struct DbTransactionCollectionsJson
{
    public IReadOnlyCollection<string>? Read { get; init; }

    public IReadOnlyCollection<string>? Write { get; init; }

    public IReadOnlyCollection<string>? Exclusive { get; init; }
}