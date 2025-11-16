using MyStore.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace MyStore.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        // Initialize Database
        InitializeDatabaseAsync().FireAndForget();
    }

    private async Task InitializeDatabaseAsync()
    {
        try
        {
            var dbContextFactory = ServiceHelper.GetService<IDbContextFactory<AppDbContext>>();
            using var context = dbContextFactory.CreateDbContext();

            // Create tables if not exist
            await context.Database.EnsureCreatedAsync();

            // Seed sample data if empty
            if (!context.Products.Any())
            {
                var sampleProducts = Core.Models.Product.GetSampleData();
                context.Products.AddRange(sampleProducts);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Database initialization error: {ex.Message}");
        }
    }
}
