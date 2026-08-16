using FinTrack.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Api.DTOs.Transactions;

public class CreateTransactionDto
{
    public int PortfolioId { get; set; }

    public int StockId { get; set; }

    public TransactionType Type { get; set; }

    [Range(0.000001, double.MaxValue)]
    public decimal Quantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal PricePerShare { get; set; }
}