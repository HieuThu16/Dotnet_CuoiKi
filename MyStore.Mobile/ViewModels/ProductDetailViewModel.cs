using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyStore.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace MyStore.Mobile.ViewModels;

/// <summary>
/// ViewModel for Product Detail Page
/// </summary>
[QueryProperty(nameof(ProductId), "id")]
public partial class ProductDetailViewModel : ViewModelBase
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly ICartService _cartService;

    [ObservableProperty]
    private Product? selectedProduct;

    [ObservableProperty]
    private int quantity = 1;

    [ObservableProperty]
    private int productId;

    partial void OnProductIdChanged(int value)
    {
        LoadProductAsync(value).FireAndForget();
    }

    public ProductDetailViewModel()
    {
        _dbContextFactory = ServiceHelper.GetService<IDbContextFactory<AppDbContext>>()!;
        _cartService = ServiceHelper.GetService<ICartService>()!;
    }

    private async Task LoadProductAsync(int id)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            using var context = _dbContextFactory.CreateDbContext();
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            SelectedProduct = product;
            Quantity = 1;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load product: {ex.Message}";
            Debug.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void IncreaseQuantity()
    {
        if (SelectedProduct != null && Quantity < SelectedProduct.Stock)
        {
            Quantity++;
        }
    }

    [RelayCommand]
    private void DecreaseQuantity()
    {
        if (Quantity > 1)
        {
            Quantity--;
        }
    }

    [RelayCommand]
    private async Task AddToCartAsync()
    {
        try
        {
            if (SelectedProduct == null)
                return;

            IsLoading = true;
            ErrorMessage = null;

            await _cartService.AddItemAsync(SelectedProduct, Quantity);

            // Show success message
            await Application.Current!.MainPage!.DisplayAlert(
                "Success",
                $"{SelectedProduct.Name} added to cart!",
                "OK"
            );

            // Navigate back
            await Shell.Current.GoToAsync("../");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to add to cart: {ex.Message}";
            await Application.Current!.MainPage!.DisplayAlert("Error", ErrorMessage, "OK");
            Debug.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("../");
    }
}
