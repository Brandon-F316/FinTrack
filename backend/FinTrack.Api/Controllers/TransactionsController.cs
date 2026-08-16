using FinTrack.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTrack.Api.Models;
using FinTrack.Api.DTOs.Transactions;
using FinTrack.Api.Models.Enums;


namespace FinTrack.Api.Controllers;

    
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly FinTrackDbContext _context;

    public TransactionsController(FinTrackDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
    {
        var transactions = await _context.Transactions
            .Select(transaction => new TransactionDto
            {
                Id = transaction.Id,
                PortfolioId = transaction.PortfolioId,
                StockId = transaction.StockId,
                Type = transaction.Type,
                Quantity = transaction.Quantity,
                PricePerShare = transaction.PricePerShare,
                TransactionDate = transaction.TransactionDate
            })
            .ToListAsync();
        return transactions;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
    {
        var transaction = await _context.Transactions
            .Where(transaction => transaction.Id == id)
            .Select(transaction => new TransactionDto
            {
                Id = transaction.Id,
                PortfolioId = transaction.PortfolioId,
                StockId = transaction.StockId,
                Type = transaction.Type,
                Quantity = transaction.Quantity,
                PricePerShare = transaction.PricePerShare,
                TransactionDate = transaction.TransactionDate
            })
            .FirstOrDefaultAsync();

        if (transaction == null)
        {
            return NotFound();  
        }

        return transaction;
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> CreateTransaction(CreateTransactionDto dto)
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

        if (dto.Type == TransactionType.Buy)
        {
            if (existingHolding != null)
            {
                existingHolding.Quantity += dto.Quantity;
            }

            else
            {
                var holding = new Holding
                {
                    PortfolioId = dto.PortfolioId,
                    StockId = dto.StockId,
                    Quantity = dto.Quantity

                };
                _context.Holdings.Add(holding);
            }
        }

        else if (dto.Type == TransactionType.Sell)
        {
            if (existingHolding == null)
            {
                return BadRequest("Cannot sell a stock that is not held.");
            }

            if(existingHolding.Quantity < dto.Quantity)
            {
                return BadRequest("Not enough shares to make this Transaction");
            }

            existingHolding.Quantity -= dto.Quantity;

            if (existingHolding.Quantity == 0)
            {
                _context.Holdings.Remove(existingHolding);
            }
        }

        if (dto.Type != TransactionType.Buy &&
            dto.Type != TransactionType.Sell)
        {
            return BadRequest("Invalid Transaction Type");
        }

        var transaction = new Transaction
        {
            PortfolioId = dto.PortfolioId,
            StockId = dto.StockId,
            Type = dto.Type,
            Quantity = dto.Quantity,
            PricePerShare = dto.PricePerShare,
            TransactionDate = DateTime.UtcNow
        };

        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();

        var transactionDto = new TransactionDto
        {
            Id = transaction.Id,
            PortfolioId = transaction.PortfolioId,
            StockId = transaction.StockId,
            Type = transaction.Type,
            Quantity = transaction.Quantity,
            PricePerShare = transaction.PricePerShare,
            TransactionDate = transaction.TransactionDate
        };

        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transactionDto);
    }

}

