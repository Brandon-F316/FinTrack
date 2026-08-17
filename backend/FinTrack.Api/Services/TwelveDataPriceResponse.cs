using System.Text.Json.Serialization;

namespace FinTrack.Api.Services;

public class TwelveDataPriceResponse
{
    [JsonPropertyName("price")]
    public string? Price { get; set; }
}