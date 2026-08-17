namespace FinTrack.Api.DTOs.Portfolios;

public class PortfolioHoldingSummaryDto
{
    public int StockId { get; set; }

    public string Symbol { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal CurrentPrice { get; set; }

    public decimal MarketValue { get; set; }
}