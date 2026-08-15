using FinTrack.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTrack.Api.Models;
using FinTrack.Api.DTOs.Portfolios;


namespace FinTrack.Api.Controllers;


[ApiController]
[Route("api/[controller]")]

public class PortfoliosController : ControllerBase
{
    private readonly FinTrackDbContext _context;

    public PortfoliosController(FinTrackDbContext context)
    {
        _context = context; 
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PortfolioDto>>> GetPortfolios()
    {
        var portfolios = await _context.Portfolios
            .Select(portfolio => new PortfolioDto
            {
                Id = portfolio.Id,
                Name = portfolio.Name

            })
            .ToListAsync();
        return portfolios;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PortfolioDto>> GetPortfolio(int id)
    {
        var portfolio = await _context.Portfolios
            .Where(portfolio => portfolio.Id == id)
            .Select(portfolio => new PortfolioDto
            {
                Id = portfolio.Id,
                Name = portfolio.Name
                
            })
            .FirstOrDefaultAsync();

        if (portfolio == null)
        {
            return NotFound(); 
        }

        return portfolio;
    }

    [HttpPost]
    public async Task<ActionResult<PortfolioDto>> CreatePortfolio(CreatePortfolioDto dto)
    {
        var portfolio = new Portfolio
        {
            Name = dto.Name
        };
        _context.Portfolios.Add(portfolio);
        await _context.SaveChangesAsync();

        var portfolioDto = new PortfolioDto
        {
            Id = portfolio.Id,
            Name = portfolio.Name
        };
        return CreatedAtAction(nameof(GetPortfolio), new { id = portfolio.Id }, portfolioDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePortfolio(int id, UpdatePortfolioDto dto)
    {
        var portfolio = await _context.Portfolios.FindAsync(id);

        if (portfolio == null)
        {
            return NotFound();
        }

        portfolio.Name = dto.Name; 

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePortfolio(int id)
    {
        var portfolio = await _context.Portfolios.FindAsync(id);

        if(portfolio == null)
        {
            return NotFound();
        }

        _context.Portfolios.Remove(portfolio);

        await _context.SaveChangesAsync();

        return NoContent();
    }


}