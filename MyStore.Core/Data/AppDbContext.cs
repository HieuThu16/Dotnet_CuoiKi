// File: MyStore.Core/Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using MyStore.Core.Models;
using System.IO;

namespace MyStore.Core.Data;

/// <summary>
/// EF Core DbContext for SQLite database
/// Manages Cart, Product, and Order entities
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<OrderModel> Orders { get; set; } = null!;

    private readonly string _databasePath;

    public AppDbContext() : this(new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite($"Data Source={GetDatabasePath()}")
        .Options)
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        _databasePath = GetDatabasePath();
    }

    /// <summary>
    /// Get platform-specific database file path
    /// </summary>
    private static string GetDatabasePath()
    {
        string dbPath;

#if __ANDROID__
        dbPath = Path.Combine(
            Android.App.Application.Context.GetExternalFilesDir(null)?.AbsolutePath
            ?? Android.App.Application.Context.FilesDir.AbsolutePath,
            "mystore.db"
        );
#elif __IOS__
        dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "mystore.db"
        );
#elif WINDOWS_UWP || __WASM__
        // For WASM, use localStorage or in-memory; for UWP use AppData
        dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MyStore",
            "mystore.db"
        );
#else
        // Default: Local app data for Windows/Desktop
        dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MyStore",
            "mystore.db"
        );
#endif

        // Ensure directory exists
        var dir = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        return dbPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure CartItem
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')");
            entity.HasIndex(e => e.ProductId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // Configure Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.HasIndex(e => e.Name);
        });

        // Configure OrderModel
        modelBuilder.Entity<OrderModel>(entity =>
        {
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.OrderId).IsRequired().HasMaxLength(32);
            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(256);
            entity.Property(e => e.CustomerAddress).IsRequired().HasMaxLength(512);
            entity.Property(e => e.CustomerPhone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Total).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.Items).HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                v => string.IsNullOrEmpty(v)
                    ? new List<OrderItemDto>()
                    : System.Text.Json.JsonSerializer.Deserialize<List<OrderItemDto>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new()
            );
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.Status);
        });
    }

    /// <summary>
    /// Initialize database and seed data if empty
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            // Create database and tables - use migrations
            await Database.MigrateAsync();

            // Fallback to EnsureCreated if no migrations
            if (!Database.GetPendingMigrations().Any())
            {
                await Database.EnsureCreatedAsync();
            }

            // Seed sample products if Products table is empty
            if (!Products.Any())
            {
                Products.AddRange(Product.GetSampleData());
                await SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
            // Try EnsureCreated as final fallback
            try
            {
                await Database.EnsureCreatedAsync();
            }
            catch { }
        }
    }

    /// <summary>
    /// Clear all cart items
    /// </summary>
    public async Task ClearCartAsync()
    {
        CartItems.RemoveRange(CartItems);
        await SaveChangesAsync();
    }

    /// <summary>
    /// Get total items in cart
    /// </summary>
    public async Task<int> GetCartTotalQuantityAsync()
    {
        return await CartItems.SumAsync(c => c.Quantity);
    }

    /// <summary>
    /// Get total price in cart
    /// </summary>
    public async Task<decimal> GetCartTotalPriceAsync()
    {
        return await CartItems.SumAsync(c => c.UnitPrice * c.Quantity);
    }
}
