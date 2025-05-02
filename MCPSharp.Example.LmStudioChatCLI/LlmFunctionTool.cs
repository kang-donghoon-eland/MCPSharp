using System.Runtime.CompilerServices;

namespace MCPSharp.Example.LmStudioChatCLI
{
    
    public class LlmFunctionTool
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public LlmFunctionToolParameters Parameters { get; set; }

               public LlmFunctionTool()
        {
        }
    }
}