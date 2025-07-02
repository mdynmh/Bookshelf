using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Bookshelf.Services;
using Bookshelf.ViewModels;
using Bookshelf.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Bookshelf;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.RemoveAt(0);

        var serviceCollection = new ServiceCollection();

        var apiService = new ApiService("https://localhost:5047/");
        serviceCollection.AddSingleton(apiService)
            .AddSingleton<AuthService>()
            .AddSingleton<UserService>()
            .AddSingleton<BookService>()
            .AddSingleton<AuthorService>()
            .AddSingleton<IssuedBookService>()
            .AddSingleton<NavigationService>()
            .AddSingleton<UserBookService>()
            .AddSingleton<BookmarkService>();

        serviceCollection.AddSingleton<MainWindowViewModel>()
            .AddTransient<LoginViewModel>()
            .AddTransient<MainViewModel>()
            .AddTransient<BookViewModel>()
            .AddTransient<ReadingBookViewModel>()
            .AddTransient<MyBooksViewModel>()
            .AddTransient<MobileMainViewModel>();
        
        Services = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = App.Services.GetRequiredService<MainWindowViewModel>()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = App.Services.GetRequiredService<MobileMainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
