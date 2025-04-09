using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPSharp.Example.LmStudioChatCLI
{
    internal class LlmRequest
    {
        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; }

        [JsonProperty(PropertyName = "stream")]
        public bool Stream { get; set; }

        [JsonProperty(PropertyName = "messages")]
        public List<LlmMessage> Messages { get; set; }

        [JsonProperty(PropertyName = "prompt")]
        public string Prompt { get; set; }


        public static LlmRequest Create(string model, string languagePrompt, string systemprompt, string assistantprompt, string userCode, string manualRequest = "")
        {
            var llmRequest = new LlmRequest()
            {
                Model = model,
                //시스템 프롬프트
                Prompt = "[REQUEST]: " + manualRequest,
                Stream = false,
                Messages = new List<LlmMessage>()
            };

            if (!string.IsNullOrWhiteSpace(systemprompt))
                llmRequest.Messages.Add(LlmMessage.CreateSystemMessage(systemprompt));
            if (!string.IsNullOrWhiteSpace(assistantprompt))
                llmRequest.Messages.Add(LlmMessage.CreateAssistantMessage(assistantprompt));
            if (!string.IsNullOrWhiteSpace(languagePrompt))
                llmRequest.Messages.Add(LlmMessage.CreateAssistantMessage(languagePrompt));
            if(!string.IsNullOrWhiteSpace(userCode))
                llmRequest.Messages.Add(LlmMessage.CreateUserMessage(userCode));
            if (!string.IsNullOrWhiteSpace(manualRequest))
                llmRequest.Messages.Add(LlmMessage.CreateUserMessage("[REQUEST]: " + manualRequest));

            return llmRequest;
        }
    }

    internal class LlmMessage
    {
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        public static LlmMessage CreateAssistantMessage(string content)
        {
            return new LlmMessage()
            {
                Role = "assistant",
                Content = content
            };
        }

        public static LlmMessage CreateUserMessage(string content)
        {
            return new LlmMessage()
            {
                Role = "user",
                Content = content
            };
        }

        public static LlmMessage CreateSystemMessage(string content)
        {
            return new LlmMessage()
            {
                Role = "system",
                Content = content
            };
        }
    }
}
