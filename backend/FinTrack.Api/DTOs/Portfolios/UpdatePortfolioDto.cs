using System.ComponentModel.DataAnnotations;

namespace FinTrack.Api.DTOs.Portfolios;

public class UpdatePortfolioDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}