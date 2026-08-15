using System.ComponentModel.DataAnnotations;

namespace FinTrack.Api.DTOs.Stocks;

public class UpdateStockDto
{
    [Required]
    [StringLength(10)]
    public string Symbol { get; set; }

    [Required]
    [StringLength(200)]
    public string CompanyName { get; set; }
}