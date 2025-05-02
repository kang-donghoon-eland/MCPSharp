using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MCPSharp.Example.LmStudioChatCLI
{


    public sealed class LlmFunctionToolParameters
    {
        public string Type { get; set; } = "object";

        public IDictionary<string, JsonElement> Properties { get; set; }

        public IList<string>? Required { get; set; }

        
        public LlmFunctionToolParameters()
        {
        }
    }
}