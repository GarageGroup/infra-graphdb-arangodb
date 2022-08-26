namespace GGroupp.Infra.ArangoDb;

public sealed record class DbTransactionRollbackIn
{
    public DbTransactionRollbackIn(string id)
        =>
        Id = id ?? string.Empty;

    public string Id { get; }
}