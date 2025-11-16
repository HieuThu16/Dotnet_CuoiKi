using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyStore.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace MyStore.Mobile.ViewModels;

/// <summary>
/// ViewModel for Products List Page
/// </summary>
public partial class ProductsViewModel : ViewModelBase
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    [ObservableProperty]
    private ObservableCollection<Product> products = [];

    [ObservableProperty]
    private int cartItemCount = 0;

    public ProductsViewModel()
    {
        _dbContextFactory = ServiceHelper.GetService<IDbContextFactory<AppDbContext>>()!;
    }

    public override async Task InitializeAsync()
    {
        await LoadProductsAsync();
        UpdateCartBadge();
    }

    [RelayCommand]
    private async Task LoadProductsAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            using var context = _dbContextFactory.CreateDbContext();
            var productList = await context.Products.ToListAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Products.Clear();
                foreach (var product in productList)
                {
                    Products.Add(product);
                }
            });
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load products: {ex.Message}";
            Debug.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SelectProductAsync(Product product)
    {
        if (product == null)
            return;

        await Shell.Current.GoToAsync($"productDetail?id={product.Id}");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadProductsAsync();
    }

    private void UpdateCartBadge()
    {
        try
        {
            var cartService = ServiceHelper.GetService<ICartService>();
            if (cartService != null)
            {
                // Get cart count (this is fire-and-forget in background)
                cartService.GetTotalQuantityAsync()
                    .ContinueWith(task =>
                    {
                        if (task.IsCompletedSuccessfully)
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                CartItemCount = task.Result;
                            });
                        }
                    });
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating cart badge: {ex.Message}");
        }
    }
}
