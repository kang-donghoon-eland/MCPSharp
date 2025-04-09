using Microsoft.Extensions.AI;

namespace MCPSharp.Example.LmStudioChatCLI
{
    internal class ChatRequest
    {
        public ChatRequest(IList<ChatMessage> chatMessages, ChatOptions? options)
        {
            ChatMessages = chatMessages;
            Options = options;
        }

        public IList<ChatMessage> ChatMessages { get; }
        public ChatOptions? Options { get; }
    }
}