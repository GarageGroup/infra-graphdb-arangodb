namespace GGroupp.Infra.ArangoDb;

public enum DbTransactionRollbackFailureCode
{
    Unknown,

    TransactionNotFound,

    AlreadyCommitted
}