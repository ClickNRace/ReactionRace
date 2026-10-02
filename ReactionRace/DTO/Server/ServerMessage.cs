using System.Text.Json.Serialization;

namespace ReactionRace.DTO.Server
{
    public class ServerMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}
