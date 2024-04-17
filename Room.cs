using System.Text.Json.Serialization;

namespace Classes
{
    public record Room
    {
        [JsonPropertyName("roomId")]
        public string RoomId { get; init; }

        [JsonPropertyName("roomName")]
        public string RoomName { get; init; }

        [JsonPropertyName("capacity")]
        [JsonConverter(typeof(StringToIntConverter))]
        public int Capacity { get; init; }
    }
}
