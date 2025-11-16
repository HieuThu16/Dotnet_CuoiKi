using CommunityToolkit.Mvvm.ComponentModel;

namespace MyStore.Mobile.ViewModels;

/// <summary>
/// Base class for all ViewModels using MVVM Toolkit
/// </summary>
public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}
