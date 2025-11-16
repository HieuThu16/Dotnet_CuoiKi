using MyStore.Core.Data;
using MyStore.Core.Services;
using MyStore.Mobile.ViewModels;
using MyStore.Mobile.Views;
using MyStore.Mobile.Converters;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm;

namespace MyStore.Mobile;

public static class MauiProgram
{
    public static void ConfigureServices(IServiceCollection services)
    {
        // Database Configuration
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "mystore.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

        // Register AppDbContext
        services.AddSingleton<IDbContextFactory<AppDbContext>>(
            new DbContextFactory(options)
        );

        // Register Core Services
        services.AddSingleton<ICartService, CartService>();
        services.AddSingleton<CartService>();

        // Register ViewModels
        services.AddSingleton<ProductsViewModel>();
        services.AddSingleton<ProductDetailViewModel>();
        services.AddSingleton<CartViewModel>();
        services.AddSingleton<CheckoutViewModel>();

        // Register Views
        services.AddSingleton<ProductsPage>();
        services.AddSingleton<ProductDetailPage>();
        services.AddSingleton<CartPage>();
        services.AddSingleton<CheckoutPage>();

        // Register App Shell
        services.AddSingleton<AppShell>();
    }
}

/// <summary>
/// DbContext Factory for Dependency Injection
/// </summary>
public class DbContextFactory : IDbContextFactory<AppDbContext>
{
    private readonly DbContextOptions<AppDbContext> _options;

    public DbContextFactory(DbContextOptions<AppDbContext> options)
    {
        _options = options;
    }

    public AppDbContext CreateDbContext()
    {
        return new AppDbContext(_options);
    }
}
