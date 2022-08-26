using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.ArangoDb;

partial class ArangoDbGraphApi
{
    public ValueTask<Result<Unit, Failure<DbTransactionRollbackFailureCode>>> RollbackTransactionAsync(
        DbTransactionRollbackIn input, CancellationToken cancellationToken)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<Unit, Failure<DbTransactionRollbackFailureCode>>>(cancellationToken);
        }

        return InnerRollbackTransactionAsync(input, cancellationToken);
    }

    private async ValueTask<Result<Unit, Failure<DbTransactionRollbackFailureCode>>> InnerRollbackTransactionAsync(
        DbTransactionRollbackIn input, CancellationToken cancellationToken)
    {
        using var httpClient = httpMessageHandler.CreateHttpClient(option);

        var uri = string.Format(TransactionApiPathTemplate, WebUtility.UrlEncode(input.Id));
        var result = await httpClient.SendAsync(HttpMethod.Delete, uri, cancellationToken).ConfigureAwait(false);

        return result.MapFailure(MapFailure);

        static Failure<DbTransactionRollbackFailureCode> MapFailure(DbFailureJson dbFailure)
            =>
            dbFailure.ErrorNum switch
            {
                ErrorArangoTransactionNotFound => new(DbTransactionRollbackFailureCode.TransactionNotFound, dbFailure.ErrorMessage),
                ErrorArangoTransactionDisallowedOperation => new(DbTransactionRollbackFailureCode.AlreadyCommitted, dbFailure.ErrorMessage),
                _ => new(DbTransactionRollbackFailureCode.Unknown, dbFailure.ErrorMessage)
            };
    }
}