namespace GGroupp.Infra.ArangoDb;

internal readonly record struct DbTransactionJsonOut
{
    public DbTransactionResultJson Result { get; init; }
}