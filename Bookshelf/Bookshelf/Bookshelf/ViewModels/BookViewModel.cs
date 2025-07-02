using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookshelf.Dtos;
using Bookshelf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bookshelf.ViewModels
{
    public partial class BookViewModel : ViewModelBase, INavigationAware
    {
        private readonly NavigationService _nav;
        private readonly IssuedBookService _issuedBookService;
        private readonly UserBookService _userBookService;
        private readonly AuthService _auth;

        private int _availableCount;

        [ObservableProperty]
        private BookDto _book = new();

        [ObservableProperty]
        public string? _dueDateText;
        public bool IsNotAvailableReading => !string.IsNullOrEmpty(Book.FileUrl);

        public string AvailabilityText =>
            _availableCount > 0
              ? $"в наличии: {_availableCount} из {Book.TotalCopies}"
              : "нет в наличии";

        public BookViewModel(NavigationService nav
            , IssuedBookService issuedBookService
            , AuthService auth
            , UserBookService userBookService)
        {
            _nav = nav;
            _issuedBookService = issuedBookService;
            _auth = auth;

            LoadAvailabilityAsync();
            _userBookService = userBookService;
        }

        public void OnNavigatedTo(object? parameter)
        {
            if(parameter is not null)
            {
                Book = (BookDto)parameter;
            }
        }

        private async Task LoadAvailabilityAsync()
        {
            var allIssued = (await _issuedBookService.GetAllIssuedBooksAsync()).Where(i=> i.ReturnedAt == null);

            int issuedCount = allIssued
                .Count(i => i.BookId == Book.BookId);
            _availableCount = Book.TotalCopies - issuedCount;
            OnPropertyChanged(nameof(AvailabilityText));

            var dueDate = allIssued.Where(i=>i.BookId == Book.BookId && i.UserId == _auth.CurrentUserId && i.ReturnedAt == null)
                .FirstOrDefault()?.DueDate.ToShortDateString();

            if(dueDate != null)
            {
                DueDateText = $"Нужно вернуть до {dueDate}";
            }
        }

        [RelayCommand]
        private async Task NavigateToReadingAsync()
        {
            int userId = _auth.CurrentUserId;

            var userBook = await _userBookService
                .GetUserBookAsync(userId, Book.BookId)
                ?? await _userBookService.AddUserBookAsync(new UserBookCreateDto
                {
                    UserId = userId,
                    BookId = Book.BookId,
                    CurrentPage = 1
                });

            _nav.NavigateTo<ReadingBookViewModel>(userBook);
        }
    }
}
