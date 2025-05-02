using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Shared.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace MCPSharp.Example.LmStudioChatCLI
{
    public sealed class LmStudioChatClient : IChatClient, IDisposable
    {
        private static readonly JsonElement _schemalessJsonResponseFormatValue = JsonDocument.Parse("\"json\"").RootElement;
        private readonly ChatClientMetadata _metadata;
        private readonly Uri _apiChatEndpoint;
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _toolCallJsonSerializerOptions = AIJsonUtilities.DefaultOptions;

        private readonly Random _random = new Random();
        private readonly Uri _serviceEndpoint;
        private readonly string _modelId;
        //private readonly ChatClientMetadata _metadata;

        public LmStudioChatClient(string endpoint, string? modelId = null, HttpClient? httpClient = null) : this(new Uri(endpoint), modelId, httpClient)
        {
            _serviceEndpoint = new Uri(endpoint);
            _modelId = modelId ?? throw new ArgumentNullException(nameof(modelId)); // Ensure _modelId is not null
            _metadata = new ChatClientMetadata("LmStudioChatClient", new Uri(endpoint), modelId);
        }

        public LmStudioChatClient(Uri endpoint, string? modelId = null, HttpClient? httpClient = null)
        {
            //Throw.IfNull<Uri>(endpoint, nameof(endpoint));
            if (modelId != null)
                if (endpoint == null)
                    throw new ArgumentNullException(nameof(endpoint));
            if (modelId != null && string.IsNullOrWhiteSpace(modelId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(modelId));
                //Throw.IfNullOrWhitespace(modelId, nameof(modelId));
            this._apiChatEndpoint = new Uri(endpoint, "chat/completions");
            this._httpClient = httpClient ?? LmStudioUtilities.SharedClient;
            this._metadata = new ChatClientMetadata("lmstudio", endpoint, modelId);
        }

        public JsonSerializerOptions ToolCallJsonSerializerOptions
        {
            get => this._toolCallJsonSerializerOptions;
            set
            {
                this._toolCallJsonSerializerOptions = value as JsonSerializerOptions;
                if(this._toolCallJsonSerializerOptions == null)
                    throw new ArgumentException(string.Format($"value : {value}", nameof(value)));
            }
        }

        public async Task<ChatResponse> GetResponseAsync(
        IList<ChatMessage> chatMessages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
        {
            LlmRequest req = new LlmRequest();
            req.Model = this._modelId;
            req.Messages = new List<LlmMessage>();
            //req.Tools = options?.Tools?.Select(t => LlmTool());


            foreach (var chat in chatMessages)
            {
                req.Messages.Add(new LlmMessage()
                {
                    Role = chat.Role.ToString().ToLower(),
                    Content = chat.Text?? string.Empty
                });
            }
            req.Prompt = "use the model identifier from LM Studio here";

            HttpResponseMessage httpResponse = await this._httpClient.PostAsJsonAsync<LlmRequest>(
                _apiChatEndpoint,
                req,
                cancellationToken: cancellationToken);
            var response = httpResponse.IsSuccessStatusCode
                ? await httpResponse.Content.ReadAsStringAsync(cancellationToken: cancellationToken)
                : throw new InvalidOperationException("LLM error: " + httpResponse.StatusCode);

            var chosenResponse = "";
            var choices = JObject.Parse(response)["choices"];
            if (!choices.Any())
            {
                chosenResponse = string.Empty;
            }
            else
            {
                chosenResponse = choices[0]["message"]["content"].ToString();
            }
            return new(new ChatMessage(ChatRole.Assistant, chosenResponse));
        }


        public object? GetService(Type serviceType, object? key = null) =>
           key is not null ? null :
           serviceType == typeof(ChatClientMetadata) ? _metadata :
           serviceType?.IsInstanceOfType(this) is true ? this :
           null;

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IList<ChatMessage> chatMessages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            // Simulate streaming by yielding messages one by one
            yield return new ChatResponseUpdate
            {
                Role = ChatRole.Assistant,
                Text = "This is the first part of the stream."
            };

            await Task.Delay(300, cancellationToken);

            yield return new ChatResponseUpdate()
            {
                Role = ChatRole.Assistant,
                Text = "This is the second part of the stream."
            };
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        
    }
}
