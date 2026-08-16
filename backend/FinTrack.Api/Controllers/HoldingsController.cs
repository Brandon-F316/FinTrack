using FinTrack.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTrack.Api.Models;
using FinTrack.Api.DTOs.Holdings;


namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class HoldingsController : ControllerBase
{
    private readonly FinTrackDbContext _context;

    public HoldingsController(FinTrackDbContext context)
    {
        _context = context;
    } 

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HoldingDto>>> GetHoldings()
    {
        var holdings = await _context.Holdings
            .Select(holding => new HoldingDto
            {
                Id = holding.Id,
                PortfolioId = holding.PortfolioId,
                StockId = holding.StockId,
                Quantity = holding.Quantity
            }) 
            .ToListAsync();

            return holdings;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HoldingDto>> GetHolding(int id)
    {
        var holding = await _context.Holdings
            .Where(holding => holding.Id == id)
            .Select(holding => new HoldingDto
            {
                Id = holding.Id,
                PortfolioId = holding.PortfolioId,
                StockId = holding.StockId,
                Quantity = holding.Quantity
            })
            .FirstOrDefaultAsync();

        if (holding == null)
        {
            return NotFound(); 
        }

        return holding;
    }

    [HttpPost]
    public async Task<ActionResult<HoldingDto>> CreateHolding(CreateHoldingDto dto)
    {
        var portfolioExists = await _context.Portfolios
            .AnyAsync(p => p.Id == dto.PortfolioId);

        if (!portfolioExists)
        {
            return NotFound("Portfolio not found.");
        }

        var stockExists = await _context.Stocks
            .AnyAsync(s => s.Id == dto.StockId);

        if (!stockExists)
        {
            return NotFound("Stock not found.");
        }

        var existingHolding = await _context.Holdings
            .FirstOrDefaultAsync(h =>
                h.PortfolioId == dto.PortfolioId &&
                h.StockId == dto.StockId);

        if (existingHolding != null)
        {
            existingHolding.Quantity += dto.Quantity;
            
            await _context.SaveChangesAsync();

            var existingHoldingDto = new HoldingDto
            {
                Id = existingHolding.Id,
                PortfolioId = existingHolding.PortfolioId,
                StockId = existingHolding.StockId,
                Quantity = existingHolding.Quantity
            };
            
            return Ok(existingHoldingDto);
        }


        var holding = new Holding
        {
            StockId = dto.StockId,
            PortfolioId = dto.PortfolioId,
            Quantity = dto.Quantity
        };

        _context.Holdings.Add(holding);
        await _context.SaveChangesAsync();

        var holdingDto = new HoldingDto
        {
            Id = holding.Id,
            PortfolioId = holding.PortfolioId,
            StockId = holding.StockId,
            Quantity = holding.Quantity
        };

        return CreatedAtAction(nameof(GetHolding), new { id = holding.Id }, holdingDto);
    }

}
