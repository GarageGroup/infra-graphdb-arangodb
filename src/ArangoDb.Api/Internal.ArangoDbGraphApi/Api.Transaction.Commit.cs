using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.ArangoDb;

partial class ArangoDbGraphApi
{
    public ValueTask<Result<Unit, Failure<DbTransactionCommitFailureCode>>> CommitTransactionAsync(
        DbTransactionCommitIn input, CancellationToken cancellationToken)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<Unit, Failure<DbTransactionCommitFailureCode>>>(cancellationToken);
        }

        return InnerCommitTransactionAsync(input, cancellationToken);
    }

    private async ValueTask<Result<Unit, Failure<DbTransactionCommitFailureCode>>> InnerCommitTransactionAsync(
        DbTransactionCommitIn input, CancellationToken cancellationToken)
    {
        using var httpClient = httpMessageHandler.CreateHttpClient(option);

        var uri = string.Format(TransactionApiPathTemplate, WebUtility.UrlEncode(input.Id));
        var result = await httpClient.SendAsync(HttpMethod.Put, uri, cancellationToken).ConfigureAwait(false);

        return result.MapFailure(MapFailure);

        static Failure<DbTransactionCommitFailureCode> MapFailure(DbFailureJson dbFailure)
            =>
            dbFailure.ErrorNum switch
            {
                ErrorArangoTransactionNotFound => new(DbTransactionCommitFailureCode.TransactionNotFound, dbFailure.ErrorMessage),
                ErrorArangoTransactionAborted => new(DbTransactionCommitFailureCode.AlreadyAborted, dbFailure.ErrorMessage),
                _ => new(DbTransactionCommitFailureCode.Unknown, dbFailure.ErrorMessage)
            };
    }
}