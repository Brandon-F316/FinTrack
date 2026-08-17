using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FinTrack.Api.Services;

public class StockPriceService : IStockPriceService
{
    private readonly HttpClient _httpClient;
    private readonly TwelveDataOptions _options;

    public StockPriceService(HttpClient httpClient, IOptions<TwelveDataOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }
    
    public async Task<decimal?> GetPriceAsync(string symbol)
    {
        var url = $"{_options.BaseUrl}price?symbol={symbol}&apikey={_options.ApiKey}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception( $"Twelve Data returned {response.StatusCode}: {error}");
        }

        var json = await response.Content.ReadAsStringAsync();

        var priceResponse = JsonSerializer.Deserialize<TwelveDataPriceResponse>(json);

        if (priceResponse?.Price == null)
        {
            throw new Exception($"Twelve Data did not contain a price.");
        }

        if (!decimal.TryParse(priceResponse.Price, out var price))
        {
            throw new Exception($"Could not parse a price: {priceResponse.Price}");
        }

        return price;
    }
}