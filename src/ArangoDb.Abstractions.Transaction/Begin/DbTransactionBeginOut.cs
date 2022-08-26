namespace GGroupp.Infra.ArangoDb;

public readonly record struct DbTransactionBeginOut
{
    private readonly string? id;

    public DbTransactionBeginOut(string id)
        =>
        this.id = string.IsNullOrEmpty(id) ? null : id;

    public string Id => id ?? string.Empty;
}