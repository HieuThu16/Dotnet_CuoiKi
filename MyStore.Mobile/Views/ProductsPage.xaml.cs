namespace MyStore.Mobile.Views;

public partial class ProductsPage : ContentPage
{
    public ProductsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProductsViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}
