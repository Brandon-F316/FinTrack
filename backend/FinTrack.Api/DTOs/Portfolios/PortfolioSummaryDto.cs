namespace FinTrack.Api.DTOs.Portfolios;

public class PortfolioSummaryDto
{
    public int PortfolioId { get; set; }

    public string PortfolioName { get; set; } = string.Empty;

    public decimal TotalValue { get; set; }

    public List<PortfolioHoldingSummaryDto> Holdings { get; set; } = new();
}