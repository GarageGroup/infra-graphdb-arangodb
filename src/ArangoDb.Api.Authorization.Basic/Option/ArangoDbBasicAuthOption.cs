namespace GGroupp.Infra.ArangoDb;

public sealed record class ArangoDbBasicAuthOption
{
    public ArangoDbBasicAuthOption(string userName, string password)
    {
        UserName = userName ?? string.Empty;
        Password = password ?? string.Empty;
    }

    public string UserName { get; }

    public string Password { get; }
}