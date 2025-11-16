namespace MyStore.Mobile;

public static class Program
{
    public static void Main()
    {
        CreateMauiApp().Run();
    }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit();

        // Configure Services
        MauiProgram.ConfigureServices(builder.Services);

        return builder.Build();
    }
}
