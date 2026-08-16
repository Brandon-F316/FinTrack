using FinTrack.Api.Models.Enums;

namespace FinTrack.Api.DTOs.Transactions;

public class TransactionDto
{
    public int Id { get; set; }

    public int PortfolioId { get; set; }

    public int StockId { get; set; }

    public TransactionType Type { get; set; }

    public decimal Quantity { get; set; }

    public decimal PricePerShare { get; set; }

    public DateTime TransactionDate { get; set; }
}