using System;
using System.ComponentModel;
using Bookshelf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bookshelf.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private NavigationService _navigation;

    public MainWindowViewModel(NavigationService nav)
    {
        Navigation = nav;
        Navigation.NavigateTo<LoginViewModel>();
    }

    [RelayCommand]
    private void GoBack()
    {
        Navigation.GoBack();
    }

    [RelayCommand]
    private void NavigateToMyBooks()
    {
        Navigation.NavigateTo<MyBooksViewModel>();
    }
}
