using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyStore.Mobile.ViewModels;

/// <summary>
/// ViewModel for Shopping Cart Page
/// </summary>
public partial class CartViewModel : ViewModelBase
{
    private readonly ICartService _cartService;

    [ObservableProperty]
    private ObservableCollection<CartItem> cartItems = [];

    [ObservableProperty]
    private decimal totalPrice = 0;

    [ObservableProperty]
    private int totalQuantity = 0;

    [ObservableProperty]
    private bool isEmptyCart = true;

    public CartViewModel()
    {
        _cartService = ServiceHelper.GetService<ICartService>()!;
        _cartService.CartChanged += OnCartChanged;
    }

    public override async Task InitializeAsync()
    {
        await LoadCartAsync();
    }

    [RelayCommand]
    public async Task LoadCartAsync()
    {
        try
        {
            IsLoading = true;

            var items = await _cartService.GetCartAsync();
            var total = await _cartService.GetTotalPriceAsync();
            var quantity = await _cartService.GetTotalQuantityAsync();
            var isEmpty = await _cartService.IsEmptyAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                CartItems.Clear();
                foreach (var item in items)
                {
                    CartItems.Add(item);
                }
                TotalPrice = total;
                TotalQuantity = quantity;
                IsEmptyCart = isEmpty;
            });
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load cart: {ex.Message}";
            Debug.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task UpdateQuantityAsync(CartItem item)
    {
        if (item == null)
            return;

        try
        {
            await _cartService.UpdateItemAsync(item.Id, item.Quantity);
            await LoadCartAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            Debug.WriteLine(ex);
        }
    }

    [RelayCommand]
    private async Task RemoveItemAsync(CartItem item)
    {
        if (item == null)
            return;

        var confirm = await Application.Current!.MainPage!.DisplayAlert(
            "Remove Item",
            $"Remove {item.Name} from cart?",
            "Yes",
            "No"
        );

        if (!confirm)
            return;

        try
        {
            await _cartService.RemoveItemAsync(item.Id);
            await LoadCartAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            Debug.WriteLine(ex);
        }
    }

    [RelayCommand]
    private async Task ClearCartAsync()
    {
        var confirm = await Application.Current!.MainPage!.DisplayAlert(
            "Clear Cart",
            "Remove all items from cart?",
            "Yes",
            "No"
        );

        if (!confirm)
            return;

        try
        {
            await _cartService.ClearCartAsync();
            await LoadCartAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            Debug.WriteLine(ex);
        }
    }

    [RelayCommand]
    private async Task CheckoutAsync()
    {
        if (IsEmptyCart)
        {
            await Application.Current!.MainPage!.DisplayAlert("Empty Cart", "Add items to cart first", "OK");
            return;
        }

        await Shell.Current.GoToAsync("checkout");
    }

    private void OnCartChanged(object? sender, CartChangedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await LoadCartAsync();
        });
    }
}
