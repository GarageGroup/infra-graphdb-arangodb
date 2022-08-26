namespace GGroupp.Infra.ArangoDb;

public enum DbTransactionCommitFailureCode
{
    Unknown,

    TransactionNotFound,

    AlreadyAborted
}