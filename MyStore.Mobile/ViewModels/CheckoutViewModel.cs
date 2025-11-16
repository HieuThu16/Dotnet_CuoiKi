using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyStore.Core.Models;

namespace MyStore.Mobile.ViewModels;

/// <summary>
/// ViewModel for Checkout Page
/// </summary>
public partial class CheckoutViewModel : ViewModelBase
{
    private readonly ICartService _cartService;

    [ObservableProperty]
    private string customerName = string.Empty;

    [ObservableProperty]
    private string customerAddress = string.Empty;

    [ObservableProperty]
    private string customerPhone = string.Empty;

    [ObservableProperty]
    private string orderNotes = string.Empty;

    [ObservableProperty]
    private decimal orderTotal = 0;

    [ObservableProperty]
    private int itemCount = 0;

    [ObservableProperty]
    private ObservableCollection<CartItem> orderItems = [];

    [ObservableProperty]
    private bool isFormValid = false;

    public CheckoutViewModel()
    {
        _cartService = ServiceHelper.GetService<ICartService>()!;
    }

    public override async Task InitializeAsync()
    {
        await LoadOrderSummaryAsync();
    }

    [RelayCommand]
    private async Task LoadOrderSummaryAsync()
    {
        try
        {
            var items = await _cartService.GetCartAsync();
            var total = await _cartService.GetTotalPriceAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                OrderItems.Clear();
                foreach (var item in items)
                {
                    OrderItems.Add(item);
                }
                OrderTotal = total;
                ItemCount = items.Count;
            });
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load order: {ex.Message}";
            Debug.WriteLine(ex);
        }
    }

    partial void OnCustomerNameChanged(string value) => ValidateForm();
    partial void OnCustomerAddressChanged(string value) => ValidateForm();
    partial void OnCustomerPhoneChanged(string value) => ValidateForm();

    private void ValidateForm()
    {
        IsFormValid = !string.IsNullOrWhiteSpace(CustomerName) &&
                     !string.IsNullOrWhiteSpace(CustomerAddress) &&
                     !string.IsNullOrWhiteSpace(CustomerPhone) &&
                     CustomerName.Length <= 256 &&
                     CustomerAddress.Length <= 512 &&
                     CustomerPhone.Length >= 10;
    }

    [RelayCommand]
    private async Task PlaceOrderAsync()
    {
        if (!IsFormValid)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Validation Error",
                "Please fill all required fields correctly",
                "OK"
            );
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            // Create order
            var order = new OrderModel
            {
                CustomerName = CustomerName,
                CustomerAddress = CustomerAddress,
                CustomerPhone = CustomerPhone,
                Notes = OrderNotes,
                Items = OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                }).ToList(),
                Total = OrderTotal
            };

            // In real app, this would call API
            // For now, just simulate success
            await Task.Delay(1500);

            // Clear cart
            await _cartService.ClearCartAsync();

            // Show success
            await Application.Current!.MainPage!.DisplayAlert(
                "Order Placed!",
                $"Order ID: {Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}\n\nThank you for your order!",
                "OK"
            );

            // Navigate back to products
            await Shell.Current.GoToAsync("///products");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to place order: {ex.Message}";
            await Application.Current!.MainPage!.DisplayAlert("Error", ErrorMessage, "OK");
            Debug.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("../");
    }
}
