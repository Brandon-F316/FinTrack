using FinTrack.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTrack.Api.Models;
using FinTrack.Api.DTOs.Stocks;
using FinTrack.Api.Services;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly FinTrackDbContext _context;
    private readonly IStockPriceService _stockPriceService;

    public StocksController(FinTrackDbContext context, IStockPriceService stockPriceService)
    {
        _context = context;
        _stockPriceService = stockPriceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockDto>>> GetStocks()
    {
        var stocks = await _context.Stocks
            .Select(stock => new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName
            })
            .ToListAsync();
        return stocks;
    }

    [HttpGet("price/{symbol}")]
    public async Task<ActionResult<decimal>> GetStockPrice(string symbol)
    {
        var price = await _stockPriceService.GetPriceAsync(symbol);

        if (price == null)
        {
            return NotFound("Could not retrieve stock price.");
        }

        return Ok(price);
    }

    [HttpPost]
    public async Task<ActionResult<StockDto>> CreateStock(CreateStockDto dto)
    {
        var stock = new Stock
        {
            Symbol = dto.Symbol,
            CompanyName = dto.CompanyName
        };

        _context.Stocks.Add(stock);
        
        await _context.SaveChangesAsync();

        var stockDto = new StockDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName
        };

        return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, stockDto);
    } 

    [HttpGet("{id}")]
    public async Task<ActionResult<StockDto>> GetStock(int id)
    {
        var stock = await _context.Stocks
            .Where(stock => stock.Id == id)
            .Select(stock => new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName
            })
            .FirstOrDefaultAsync();

        if (stock == null)
        {
            return NotFound();
        }

        return stock;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStock(int id, UpdateStockDto dto)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        stock.Symbol = dto.Symbol;
        stock.CompanyName = dto.CompanyName;

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