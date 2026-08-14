namespace FinTrack.Api.Models;

public class Holding
{
    public int Id { get; set; }

    public int PortfolioId { get; set; }

    public int StockId { get; set; }

    public decimal Quantity { get; set; }

    public Portfolio Portfolio { get; set; }

    public Stock Stock { get; set; }
}