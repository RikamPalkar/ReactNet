using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

/// <summary>
/// Represents the application's database context, used to interact with the database using EF Core.
/// </summary>
/// <remarks>
/// Uses a primary constructor to pass <see cref="DbContextOptions{AppDbContext}"/> to the base <see cref="DbContext"/> class.
/// </remarks>
public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts) // Primary constructor
{
    /// <summary>
    /// Gets or sets the collection of <see cref="Trade"/> entities.
    /// Represents the Trades table in the database.
    /// </summary>
    public DbSet<Trade> Trades { get; set; } = null!;

    /// <summary>
    /// Configures the EF Core model.
    /// </summary>
    /// <param name="modelBuilder">The builder used to configure entity mappings.</param>
    /// <remarks>
    /// This method is called by EF Core when building the model.
    /// Here, we seed initial data for the <see cref="Trade"/> entity using <c>HasData</c>.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Trade>().HasData(
            [
                new()
                {
                    Id = 1,
                    Commodity = "Crude Oil",
                    Quantity = 1000,
                    Price = 72.50m,
                    TradeDate = DateTime.UtcNow.AddDays(-3),
                    Counterparty = "Acme"
                }
            ]
        );
    }
}
