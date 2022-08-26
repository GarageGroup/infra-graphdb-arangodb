using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.ArangoDb;

public interface IDbTransactionSupplier
{
    ValueTask<Result<DbTransactionBeginOut, Failure<DbTransactionBeginFailureCode>>> BeginTransactionAsync(
        DbTransactionBeginIn input, CancellationToken cancellationToken);

    ValueTask<Result<Unit, Failure<DbTransactionCommitFailureCode>>> CommitTransactionAsync(
        DbTransactionCommitIn input, CancellationToken cancellationToken);

    ValueTask<Result<Unit, Failure<DbTransactionRollbackFailureCode>>> RollbackTransactionAsync(
        DbTransactionRollbackIn input, CancellationToken cancellationToken);
}