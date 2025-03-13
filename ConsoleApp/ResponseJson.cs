using System.Text.Json.Serialization;

namespace ConsoleApp;

/// <summary>
/// Модель Json ответа
/// </summary>
public class ResponseJson
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    
    [JsonPropertyName("createdAt")]
    public string CreatedAt { get; set; }
}