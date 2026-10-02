using System.Text.Json.Serialization;

namespace ReactionRace.DTO.Server
{
    public class ResultMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        public double ReactionSec { get; set; } = -1;
    }
}
