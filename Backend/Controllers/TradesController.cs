using System;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

/// <summary>
/// Handles CRUD operations for <see cref="Trade"/> entities.
/// This controller exposes REST API endpoints clients can call using HTTP methods.
/// </summary>
/// <remarks>
/// Uses dependency injection to access the EF Core database context.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class TradesController(AppDbContext db) : ControllerBase
{
    /// <summary>
    /// The EF Core database context used to access the Trades table.
    /// </summary>
    public readonly AppDbContext _db = db;

    /// <summary>
    /// Retrieves all trades sorted by date (latest first).
    /// </summary>
    /// <returns>A list of Trade objects.</returns>
    /// <remarks>
    /// **HTTP Method:** GET  
    /// **URL:** `/api/trades`  
    /// **Client Example:**  
    /// GET https://localhost:5001/api/trades
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trade>>> GetAllAsync() =>
        await _db.Trades.OrderByDescending(t => t.TradeDate).ToListAsync();

    /// <summary>
    /// Retrieves a single trade by its ID.
    /// </summary>
    /// <param name="id">Trade ID passed as a route parameter.</param>
    /// <returns>The trade if found; otherwise NotFound.</returns>
    /// <remarks>
    /// **HTTP Method:** GET  
    /// **URL:** `/api/trades/5`  
    /// **Route Parameter:** `id`  
    /// **Client Example:**  
    /// GET https://localhost:5001/api/trades/5
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<ActionResult<Trade>> Get(int id)
    {
        var trade = await _db.Trades.FindAsync(id);
        return trade == null ? (ActionResult<Trade>) NotFound() : (ActionResult<Trade>)trade;
    }

    /// <summary>
    /// Creates a new trade entry.
    /// </summary>
    /// <param name="trade">The trade object sent in the request body (JSON).</param>
    /// <returns>The newly created trade with assigned ID.</returns>
    /// <remarks>
    /// **HTTP Method:** POST  
    /// **URL:** `/api/trades`  
    /// **Body:** JSON object representing a Trade  
    /// **Client Example:**  
    /// POST https://localhost:5001/api/trades  
    /// Body:
    /// {
    ///   "commodity": "Gold",
    ///   "quantity": 200,
    ///   "price": 1850.75
    /// }
    /// </remarks>
    [HttpPost]
    public async Task<ActionResult<Trade>> Create(Trade trade)
    {
        trade.TradeDate = trade.TradeDate == default ? DateTime.UtcNow : trade.TradeDate;
        _db.Trades.Add(trade);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get),
                               new { id = trade.Id },
                               trade);
    }

    /// <summary>
    /// Updates an existing trade.
    /// </summary>
    /// <param name="id">The ID of the trade to update.</param>
    /// <param name="trade">The updated trade object sent in the body (JSON).</param>
    /// <returns>NoContent if updated; BadRequest or NotFound otherwise.</returns>
    /// <remarks>
    /// **HTTP Method:** PUT  
    /// **URL:** `/api/trades/5`  
    /// **Body:** JSON with updated fields  
    /// **Client Example:**  
    /// PUT https://localhost:5001/api/trades/5  
    /// Body:  
    /// {
    ///   "id": 5,
    ///   "commodity": "Copper",
    ///   "quantity": 3000,
    ///   "price": 4.65
    /// }
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Trade trade)
    {
        if (id != trade.Id) return BadRequest();
        _db.Entry(trade).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!_db.Trades.Any(t => t.Id == id))
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a trade using its ID.
    /// </summary>
    /// <param name="id">ID of the trade to delete.</param>
    /// <returns>NoContent if deleted; NotFound if it doesn't exist.</returns>
    /// <remarks>
    /// **HTTP Method:** DELETE  
    /// **URL:** `/api/trades/5`  
    /// **Client Example:**  
    /// DELETE https://localhost:5001/api/trades/5
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var trade = await _db.Trades.FindAsync(id);
        if (trade == null) return NotFound();

        _db.Trades.Remove(trade);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
