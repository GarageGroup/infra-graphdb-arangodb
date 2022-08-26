using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.ArangoDb;

partial class ArangoDbGraphApi
{
    public ValueTask<Result<DbTransactionBeginOut, Failure<DbTransactionBeginFailureCode>>> BeginTransactionAsync(
        DbTransactionBeginIn input, CancellationToken cancellationToken)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<DbTransactionBeginOut, Failure<DbTransactionBeginFailureCode>>>(cancellationToken);
        }

        return InnerBeginTransactionAsync(input, cancellationToken);
    }

    private async ValueTask<Result<DbTransactionBeginOut, Failure<DbTransactionBeginFailureCode>>> InnerBeginTransactionAsync(
        DbTransactionBeginIn input, CancellationToken cancellationToken)
    {
        using var httpClient = httpMessageHandler.CreateHttpClient(option);

        var dbInput = new DbTransactionJsonIn
        {
            AllowImplicit = input.AllowImplicit,
            Collections = new()
            {
                Read = input.Collections.Read,
                Write = input.Collections.Write,
                Exclusive = input.Collections.Exclusive
            },
            LockTimeout = RoundInt64(input.LockTimeout?.TotalSeconds),
            WaitForSync = input.WaitForSync
        };

        var result = await httpClient.SendDocumentAsync<DbTransactionJsonIn, DbTransactionJsonOut>(
            HttpMethod.Post, string.Format(TransactionApiPathTemplate, "begin"), dbInput, cancellationToken).ConfigureAwait(false);

        return result.Map(MapSuccess, MapFailure);

        static DbTransactionBeginOut MapSuccess(DbTransactionJsonOut dbOutput)
            =>
            new(
                id: dbOutput.Result.Id ?? string.Empty);

        static Failure<DbTransactionBeginFailureCode> MapFailure(DbFailureJson dbFailure)
            =>
            dbFailure.ErrorNum switch
            {
                ErrorArangoDataSourceNotFound or ErrorArangoTransactionUnregisteredCollection
                    => new(DbTransactionBeginFailureCode.CollectionNotFound, dbFailure.ErrorMessage),
                _
                    => new(DbTransactionBeginFailureCode.Unknown, dbFailure.ErrorMessage)
            };
    }
}