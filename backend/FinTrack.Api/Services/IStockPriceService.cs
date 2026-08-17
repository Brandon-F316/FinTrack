namespace FinTrack.Api.Services;

public interface IStockPriceService
{
    Task<decimal?> GetPriceAsync(string symbol);
}