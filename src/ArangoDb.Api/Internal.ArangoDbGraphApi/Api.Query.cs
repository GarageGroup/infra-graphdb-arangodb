using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.ArangoDb;

partial class ArangoDbGraphApi
{
    public ValueTask<Result<DbQueryOut<T>, Failure<DbQueryFailureCode>>> QueryAsync<T>(
        DbQueryIn input, CancellationToken cancellationToken)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<DbQueryOut<T>, Failure<DbQueryFailureCode>>>(cancellationToken);
        }

        return InnerQueryAsync<T>(input, cancellationToken);
    }

    private async ValueTask<Result<DbQueryOut<T>, Failure<DbQueryFailureCode>>> InnerQueryAsync<T>(
        DbQueryIn input, CancellationToken cancellationToken)
    {
        using var httpClient = httpMessageHandler.CreateHttpClient(option).AddTransactionIdHeader(input.TransactionId);

        var result = string.IsNullOrEmpty(input.CursorId) switch
        {
            true    => await CreateCursorAsync<T>(httpClient, input, cancellationToken).ConfigureAwait(false),
            _       => await ReadCursorAsync<T>(httpClient, input.CursorId, cancellationToken).ConfigureAwait(false)
        };

        return result.Map(MapSuccess, MapFailure);

        DbQueryOut<T> MapSuccess(DbCursorJsonOut<T> dbCursor)
            =>
            new(
                transactionId: input.TransactionId,
                cursorId: dbCursor.Id,
                documents: dbCursor.Result,
                cached: dbCursor.Cached,
                count: dbCursor.Count,
                hasMore: dbCursor.HasMore);

        static Failure<DbQueryFailureCode> MapFailure(DbFailureJson dbFailure)
            =>
            dbFailure.ErrorNum switch
            {
                ErrorArangoConflict or ErrorArangoUniqueConstraintViolatedPermalink => new(DbQueryFailureCode.Conflict, dbFailure.ErrorMessage),
                ErrorArangoDocumentNotFound => new(DbQueryFailureCode.DocumentNotFound, dbFailure.ErrorMessage),
                ErrorArangoDataSourceNotFound => new(DbQueryFailureCode.CollectionNotFound, dbFailure.ErrorMessage),
                ErrorArangoCursorNotFound => new(DbQueryFailureCode.CursorNotFound, dbFailure.ErrorMessage),
                ErrorArangoTransactionNotFound => new(DbQueryFailureCode.TransactionNotFound, dbFailure.ErrorMessage),
                _ => new(DbQueryFailureCode.Unknown, dbFailure.ErrorMessage)
            };
    }

    private static ValueTask<Result<DbCursorJsonOut<T>, DbFailureJson>> CreateCursorAsync<T>(
        HttpClient httpClient, DbQueryIn input, CancellationToken cancellationToken)
    {
        var dbCursor = new DbCursorJsonIn
        {
            Query = input.Query,
            BindVars = input.QueryParameters,
            Options = input.Stream is null ? null : new()
            {
                Stream = input.Stream
            },
            Count = input.Count,
            BatchSize = input.BatchSize,
            Ttl = RoundInt32(input.CursorTtl?.TotalSeconds)
        };

        return httpClient.SendDocumentAsync<DbCursorJsonIn, DbCursorJsonOut<T>>(HttpMethod.Post, CursorApiPath, dbCursor, cancellationToken);
    }

    private static ValueTask<Result<DbCursorJsonOut<T>, DbFailureJson>> ReadCursorAsync<T>(
        HttpClient httpClient, string cursorId, CancellationToken cancellationToken)
    {
        var uri = CursorApiPath + "/" + WebUtility.UrlEncode(cursorId);
        return httpClient.SendDocumentAsync<DbCursorJsonIn?, DbCursorJsonOut<T>>(HttpMethod.Put, uri, null, cancellationToken);
    }
}