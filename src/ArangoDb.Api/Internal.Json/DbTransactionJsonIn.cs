namespace GGroupp.Infra.ArangoDb;

internal readonly record struct DbTransactionJsonIn
{
    public bool AllowImplicit { get; init; }

    public DbTransactionCollectionsJson Collections { get; init; }

    public long? LockTimeout { get; init; }

    public bool? WaitForSync { get; init; }
}