using System.Text.Json.Serialization;

public record Room{
    [JsonPropertyName("roomId")]
    public string? roomId { get; init; }

    [JsonPropertyName("roomName")]
    public string? roomName { get; init; }

    [JsonPropertyName("capacity")]
    public string? capacity { get; init; }

}