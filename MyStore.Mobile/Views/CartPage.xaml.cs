namespace MyStore.Mobile.Views;

public partial class CartPage : ContentPage
{
    public CartPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CartViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}
