namespace FinTrack.Api.DTOs.Holdings;
public class HoldingDto
{
    public int Id { get; set; }

    public int PortfolioId { get; set; }

    public int StockId { get; set; }

    public decimal Quantity { get; set; }   
}