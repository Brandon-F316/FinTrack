using System.ComponentModel.DataAnnotations;
using FinTrack.Api.Models;

namespace FinTrack.Api.DTOs.Holdings;

public class CreateHoldingDto
{
    [Required]
    public int StockId { get; set; }
    
    public int PortfolioId { get; set; }

    [Range(0.000001, double.MaxValue)]
    public decimal Quantity { get; set; }
}
