namespace GGroupp.Infra.ArangoDb;

public enum DbQueryFailureCode
{
    Unknown,

    Conflict,

    DocumentNotFound,

    CollectionNotFound,

    TransactionNotFound,

    CursorNotFound
}