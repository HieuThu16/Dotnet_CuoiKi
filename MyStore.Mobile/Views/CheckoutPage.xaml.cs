namespace MyStore.Mobile.Views;

public partial class CheckoutPage : ContentPage
{
    public CheckoutPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CheckoutViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}
