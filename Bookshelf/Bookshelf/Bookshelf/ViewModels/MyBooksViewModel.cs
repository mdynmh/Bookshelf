using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookshelf.Dtos;
using Bookshelf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bookshelf.ViewModels
{
    public partial class MyBooksViewModel : ViewModelBase
    {
        private readonly IssuedBookService _issuedBookService;
        private readonly UserBookService _userBookService;
        private readonly BookService _bookService;
        private readonly AuthService _auth;

        private bool _onlineBooksSelected = true;
        public bool OnlineBooksSelected
        {
            get => _onlineBooksSelected;
            set
            {
                SetProperty(ref _onlineBooksSelected, value);
                if(_onlineBooksSelected == true)
                    BooksViewModel.LoadAsync([.. _onlineBooks]);
            }
        }

        private bool _issuedBooksSelected = false;
        public bool IssuedBooksSelected
        {
            get => _issuedBooksSelected;
            set
            {
                SetProperty(ref _issuedBooksSelected, value);
                if (_issuedBooksSelected == true)
                    BooksViewModel.LoadAsync([.. _issuedBooks]);
            }
        }

        [ObservableProperty]
        private MainViewModel _booksViewModel;

        private List<BookDto> _onlineBooks;
        private List<BookDto> _issuedBooks;

        public MyBooksViewModel(IssuedBookService issuedBookService, 
            UserBookService userBookService,
            AuthService auth,
            BookService bookService,
            MainViewModel mainViewModel)
        {
            _issuedBookService = issuedBookService;
            _userBookService = userBookService;
            _auth = auth;
            _bookService = bookService;
            BooksViewModel = mainViewModel;

            Load();
        }

        private async Task Load()
        {
            var userBooksTask = _userBookService.GetMyUserBooksAsync();
            var issuedBooksTask = _issuedBookService.GetMyIssuedBooksAsync();
            await Task.WhenAll(userBooksTask, issuedBooksTask);

            _onlineBooks = userBooksTask.Result
                              .Select(ub => ub.Book)
                              .ToList();

            _issuedBooks = issuedBooksTask.Result
                              .Select(ib => ib.Book)
                              .ToList();

            await BooksViewModel.LoadAsync([.._onlineBooks]);
        }

        [RelayCommand]
        private void SelectOnlineBooks()
        {
            OnlineBooksSelected = true;
            IssuedBooksSelected = false;
        }

        [RelayCommand]
        private void SelectIssuedBooks()
        {
            OnlineBooksSelected = false;
            IssuedBooksSelected = true;
        }
    }
}
