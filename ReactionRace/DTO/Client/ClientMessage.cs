using System.Text.Json.Serialization;

namespace ReactionRace.DTO.Client
{
    public class ClientMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}
