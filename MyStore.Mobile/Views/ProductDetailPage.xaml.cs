namespace MyStore.Mobile.Views;

public partial class ProductDetailPage : ContentPage
{
    private ProductDetailViewModel? viewModel;

    public ProductDetailPage()
    {
        InitializeComponent();
        viewModel = ServiceHelper.GetService<ProductDetailViewModel>();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (viewModel != null)
        {
            await viewModel.InitializeAsync();
        }
    }

    private void OnIncreaseQuantity(object? sender, EventArgs e)
    {
        if (viewModel != null)
        {
            viewModel.IncreaseQuantityCommand.Execute(null);
        }
    }

    private void OnDecreaseQuantity(object? sender, EventArgs e)
    {
        if (viewModel != null)
        {
            viewModel.DecreaseQuantityCommand.Execute(null);
        }
    }
}
