namespace GGroupp.Infra.ArangoDb;

internal readonly record struct DbCursorOptionJson
{
    public bool? Stream { get; init; }
}