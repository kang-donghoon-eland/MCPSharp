using Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;

namespace MCPSharp.Example.LmStudioChatCLI
{

    public class LlmTool
    {
       public string Type { get; set; }
        public LlmFunctionTool Function { get; set; }

        public LlmTool(string type, LlmFunctionTool function)
        {
            Type = type;
            Function = function;
        }
    }
}