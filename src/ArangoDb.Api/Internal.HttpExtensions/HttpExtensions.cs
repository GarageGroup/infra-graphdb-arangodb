using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra.ArangoDb;

internal static partial class HttpExtensions
{
    private const string TransactionIdHeaderName = "x-arango-trx-id";

    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private static StringContent? CreateDocumentContent<T>(T? document)
        =>
        document is null ? null : new(JsonSerializer.Serialize(document, jsonSerializerOptions), Encoding.UTF8, "application/json");

    private static async ValueTask<DbFailureJson> ReadFailureAsync(this HttpResponseMessage httpResponse, CancellationToken cancellationToken)
    {
        var dbFailure = await httpResponse.ReadContentAsync<DbFailureJson>(cancellationToken).ConfigureAwait(false);
        if (dbFailure.Code is not default(HttpStatusCode) && string.IsNullOrEmpty(dbFailure.ErrorMessage) is false)
        {
            return dbFailure;
        }

        var statusCode = dbFailure.Code is default(HttpStatusCode) ? httpResponse.StatusCode : dbFailure.Code;
        var failureMessage = dbFailure.ErrorMessage;

        if (string.IsNullOrEmpty(failureMessage))
        {
            failureMessage = $"An unexpected failure occured. Code: {statusCode}, Number: {dbFailure.ErrorNum}";
        }

        return dbFailure with
        {
            Code = statusCode,
            ErrorMessage = failureMessage
        };
    }

    private static async ValueTask<T> ReadContentAsync<T>(this HttpResponseMessage httpResponse, CancellationToken cancellationToken)
        where T : struct
    {
        var content = httpResponse.Content;

        if (content is null)
        {
            return default;
        }

        var json = await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
    }
}