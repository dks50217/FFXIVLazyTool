using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Model
{
    public class GoogleUserInfo
    {
        [JsonPropertyName("sub")] 
        public string Id { get; init; } = string.Empty;
        [JsonPropertyName("email")] 
        public string Email { get; init; } = string.Empty;
        [JsonPropertyName("name")] 
        public string Name { get; init; } = string.Empty;
        [JsonPropertyName("picture")] 
        public string? Picture { get; init; }
    }
}
