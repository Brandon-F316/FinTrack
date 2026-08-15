using System.ComponentModel.DataAnnotations;

namespace FinTrack.Api.DTOs.Portfolios;

public class CreatePortfolioDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}