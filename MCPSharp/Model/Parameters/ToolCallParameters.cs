﻿using System.Text.Json.Serialization;

namespace MCPSharp.Model.Parameters
{
    public class ToolCallParameters
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("arguments")]
        public Dictionary<string, object> Arguments { get; set; }

        [JsonPropertyName("_meta")]
        public MetaData Meta { get; set; }
    }
}
