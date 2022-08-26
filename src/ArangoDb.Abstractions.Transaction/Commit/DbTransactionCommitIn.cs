namespace GGroupp.Infra.ArangoDb;

public sealed record class DbTransactionCommitIn
{
    public DbTransactionCommitIn(string id)
        =>
        Id = id ?? string.Empty;

    public string Id { get; }
}