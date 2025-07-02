using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Bookshelf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bookshelf.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly NavigationService _nav;
    private readonly AuthService _auth;

    [ObservableProperty]
    private bool _isErrorVisible = false;
    [ObservableProperty]
    private string _textError = "";
    [ObservableProperty]
    private string _userLogin = "";
    [ObservableProperty]
    private string _userPassword = "";


    public LoginViewModel(NavigationService nav, AuthService auth)
    {
        _nav = nav;
        _auth = auth;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            await _auth.LoginAsync(UserLogin, UserPassword);
            _nav.NavigateTo<MainViewModel>();
        }
        catch (HttpRequestException ex)
        {
            if (ex.InnerException is SocketException)
            {
                TextError = "Сервер недоступен. Проверьте подключение.";
            }
            else
            {
                TextError = "Неверный логин или пароль";
            }
            IsErrorVisible = true;
        }
    }
}
