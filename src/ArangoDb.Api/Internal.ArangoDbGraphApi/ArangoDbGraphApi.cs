using System;
using System.Net.Http;

namespace GGroupp.Infra.ArangoDb;

internal sealed partial class ArangoDbGraphApi : IDbGraphApi
{
    public static ArangoDbGraphApi Create(HttpMessageHandler httpMessageHandler, ArangoDbApiOption option)
    {
        _ = httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler));
        _ = option ?? throw new ArgumentNullException(nameof(option));

        return new(httpMessageHandler, option);
    }

    private const string CursorApiPath = "_api/cursor";

    private const string TransactionApiPathTemplate = "_api/transaction/{0}";

    private const int ErrorArangoConflict = 1200;

    private const int ErrorArangoDocumentNotFound = 1202;

    private const int ErrorArangoDataSourceNotFound = 1203;

    private const int ErrorArangoUniqueConstraintViolatedPermalink = 1210;

    private const int ErrorArangoCursorNotFound = 1600;

    private const int ErrorArangoTransactionUnregisteredCollection = 1652;

    private const int ErrorArangoTransactionDisallowedOperation = 1654;

    private const int ErrorArangoTransactionAborted = 1654;

    private const int ErrorArangoTransactionNotFound = 1655;

    private readonly HttpMessageHandler httpMessageHandler;

    private readonly ArangoDbApiOption option;

    private ArangoDbGraphApi(HttpMessageHandler httpMessageHandler, ArangoDbApiOption option)
    {
        this.httpMessageHandler = httpMessageHandler;
        this.option = option;
    }

    private static int? RoundInt32(double? source)
        =>
        source is null ? null : (int)Math.Round(source.Value);

    private static long? RoundInt64(double? source)
        =>
        source is null ? null : (long)Math.Round(source.Value);
}