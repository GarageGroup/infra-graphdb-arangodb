using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra.ArangoDb;

public sealed record class DbTransactionCollections
{
    public DbTransactionCollections(
        IReadOnlyCollection<string> write,
        [AllowNull] IReadOnlyCollection<string> exclusive = null,
        [AllowNull] IReadOnlyCollection<string> read = null)
    {
        Write = write ?? Array.Empty<string>();
        Exclusive = exclusive ?? Array.Empty<string>();
        Read = read ?? Array.Empty<string>();
    }

    public IReadOnlyCollection<string> Read { get; }

    public IReadOnlyCollection<string> Write { get; }

    public IReadOnlyCollection<string> Exclusive { get; }
}