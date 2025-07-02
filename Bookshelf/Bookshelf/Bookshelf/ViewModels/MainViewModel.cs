using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Bookshelf.Dtos;
using Bookshelf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bookshelf.ViewModels;

public partial class MainViewModel : ViewModelBase, INavigationAware
{
    private readonly NavigationService _nav;
    private readonly BookService _bookService;

    private ObservableCollection<BookDto> _allBooks;
    public ObservableCollection<BookDto> AllBooks
    {
        get => _allBooks;
        set
        {
            SetProperty(ref _allBooks, value);
            OnPropertyChanged(nameof(IsVisibleSearch));
        }
    }
    [ObservableProperty]
    private ObservableCollection<BookDto> _books;
    public bool IsVisibleSearch => AllBooks?.Count() > 10;

    private string _searchText = "";
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            if (string.IsNullOrEmpty(value))
            {
                Books = AllBooks;
            }
        }
    }

    public MainViewModel(NavigationService nav, BookService bookService)
    {
        _nav = nav;
        _bookService = bookService;
    }

    public async void OnNavigatedTo(object? parameter)
    {
        await LoadAsync(parameter as ObservableCollection<BookDto>);
    }

    public async Task LoadAsync(ObservableCollection<BookDto>? allBooks = null)
    {
        if(allBooks == null)
        {
            AllBooks = [.. await _bookService.GetAllBooksAsync()];
        }
        else
        {
            AllBooks = allBooks;
        }
        Books = AllBooks;

        var semaphore = new SemaphoreSlim(5);

        var tasks = AllBooks.Select(async book =>
        {
            await semaphore.WaitAsync();
            try
            {
                book.Image = await _bookService.LoadBookCoverAsync(book.BookId);
            }
            catch
            {
                book.Image = null;
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
    }

    [RelayCommand]
    private void Search()
    {
        Books = [..AllBooks.Where(b=>b.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
            b.AuthorNames.Contains(SearchText, StringComparison.OrdinalIgnoreCase))];
    }

    [RelayCommand]
    private void NavigateToBook(BookDto book)
    {
        _nav.NavigateTo<BookViewModel>(book);
    }
}
