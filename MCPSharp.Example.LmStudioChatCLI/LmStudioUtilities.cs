using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCPSharp.Example.LmStudioChatCLI
{
    internal static class LmStudioUtilities
    {
        public static HttpClient SharedClient { get; } = new HttpClient()
        {
            Timeout = Timeout.InfiniteTimeSpan
        };

        public static void TransferNanosecondsTime<TResponse>(
          TResponse response,
          Func<TResponse, long?> getNanoseconds,
          string key,
          ref AdditionalPropertiesDictionary<long>? metadata)
        {
            long? nullable = getNanoseconds(response);
            if (!nullable.HasValue)
                return;
            long valueOrDefault = nullable.GetValueOrDefault();
            try
            {
                (metadata ?? (metadata = new AdditionalPropertiesDictionary<long>()))[key] = valueOrDefault;
            }
            catch (OverflowException ex)
            {
            }
        }

        [DoesNotReturn]
        public static async ValueTask ThrowUnsuccessfulOllamaResponseAsync(
          HttpResponseMessage response,
          CancellationToken cancellationToken)
        {
            string json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                using (JsonDocument jsonDocument = JsonDocument.Parse(json))
                {
                    JsonElement jsonElement;
                    if (jsonDocument.RootElement.TryGetProperty("error", out jsonElement))
                    {
                        if (jsonElement.ValueKind == JsonValueKind.String)
                            json = jsonElement.GetString();
                    }
                }
            }
            catch
            {
            }
            throw new InvalidOperationException("LLM error: " + json);
        }
    }
}
