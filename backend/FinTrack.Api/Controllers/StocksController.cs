using FinTrack.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTrack.Api.Models;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly FinTrackDbContext _context;

    public StocksController(FinTrackDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
    {
    return await _context.Stocks.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Stock>> CreateStock(Stock stock)
    {
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStocks), new { id = stock.Id }, stock);

    } 

    [HttpGet("{id}")]
    public async Task<ActionResult<Stock>> GetStock(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        return stock;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStock(int id, Stock stock)
    {
        if( id != stock.Id)
        {
            return BadRequest();
        }

        var existingStock = await _context.Stocks.FindAsync(id);

        if(existingStock == null)
        {
            return NotFound();
        }

        existingStock.Symbol = stock.Symbol;
        existingStock.CompanyName = stock.CompanyName;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStock(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stock);

        await _context.SaveChangesAsync();

        return NoContent();
    }

}