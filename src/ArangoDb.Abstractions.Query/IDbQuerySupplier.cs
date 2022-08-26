using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.ArangoDb;

public interface IDbQuerySupplier
{
    ValueTask<Result<DbQueryOut<T>, Failure<DbQueryFailureCode>>> QueryAsync<T>(DbQueryIn input, CancellationToken cancellationToken);
}