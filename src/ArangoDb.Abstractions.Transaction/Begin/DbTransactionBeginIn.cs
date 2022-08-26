using System;

namespace GGroupp.Infra.ArangoDb;

public sealed record class DbTransactionBeginIn
{
    public DbTransactionBeginIn(DbTransactionCollections collections, bool allowImplicit, bool? waitForSync, TimeSpan? lockTimeout)
    {
        Collections = collections ?? new(Array.Empty<string>());
        AllowImplicit = allowImplicit;
        WaitForSync = waitForSync;
        LockTimeout = lockTimeout;
    }

    public DbTransactionCollections Collections { get; }

    public bool AllowImplicit { get; }

    public bool? WaitForSync { get; }

    public TimeSpan? LockTimeout { get; }
}