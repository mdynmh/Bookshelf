using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Bookshelf.Dtos;
using Bookshelf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using UglyToad.PdfPig;

namespace Bookshelf.ViewModels
{
    public partial class ReadingBookViewModel : ViewModelBase, INavigationAware
    {
        private readonly PdfDocument _pdf;
        private readonly UserBookService _userBookService;
        private readonly BookService _bookService;
        private readonly BookmarkService _bookmarkService;
        private readonly AuthService _auth;
        private DateTime _lastPageUpdate = DateTime.Now;
        private string _filePath;


        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
                RenderPage();
                OnPropertyChanged(nameof(CurrentPageText));
                OnPropertyChanged(nameof(CanGoToPreviousPage));
                OnPropertyChanged(nameof(CanGoToNextPage));
                SaveCurrentPageAsync();
            }
        }

        public string CurrentPageText => $"{CurrentPage}/{UserBook.Book.PageCount}";

        [ObservableProperty]
        private Bitmap _page;
        [ObservableProperty]
        private ObservableCollection<BookmarkDto> _bookmarks;
        [ObservableProperty]
        private string _newBookmarkNote;

        public bool CanGoToPreviousPage => CurrentPage > 1;
        public bool CanGoToNextPage => CurrentPage < UserBook.Book.PageCount;

        public UserBookDto UserBook { get; set; }
        public List<SectionDto> Sections { get; set; }

        public ReadingBookViewModel(UserBookService userBookService,
            BookService bookService,
            BookmarkService bookmarkService,
            AuthService auth)
        {
            _userBookService = userBookService;
            _bookService = bookService;
            _bookmarkService = bookmarkService;
            _auth = auth;
        }

        private async Task SaveCurrentPageAsync()
        {
            if ((DateTime.Now - _lastPageUpdate).TotalSeconds >= 5)
            {
                UserBook.CurrentPage = CurrentPage;
                await _userBookService.UpdateIssuedBookAsync(UserBook);
                _lastPageUpdate = DateTime.Now;
            }
        }

        public async void OnNavigatedTo(object? parameter)
        {
            if (parameter is not null)
            {
                UserBook = (UserBookDto)parameter;
                CurrentPage = UserBook.CurrentPage;
                if (UserBook.Book.Sections != null)
                {
                    Sections = UserBook.Book.Sections;
                }
                if (UserBook.Bookmarks != null)
                {
                    Bookmarks = [..UserBook.Bookmarks];
                }

                var filePath = await _bookService.DownloadBookFileAsync(UserBook.BookId);
                if (filePath is not null)
                {
                    _filePath = filePath;
                }

                RenderPage();
                OnPropertyChanged(nameof(Sections));
            }
        }

        private void RenderPage()
        {
            if(_filePath == null)
            {
                return;
            }

            var opts = new PDFtoImage.RenderOptions(Dpi: 300);

            using var fs = File.OpenRead(_filePath);

            using SKBitmap skb = PDFtoImage.Conversion.ToImage(fs, options: opts, page: CurrentPage - 1);
            var wb = new WriteableBitmap(
                new PixelSize(skb.Width, skb.Height),
                new Vector(96, 96),
                Avalonia.Platform.PixelFormat.Bgra8888,
                Avalonia.Platform.AlphaFormat.Premul);

            using (var fb = wb.Lock())
            {
                var pix = skb.PeekPixels();

                if (pix != null)
                {
                    pix.ReadPixels(pix.Info, fb.Address, fb.RowBytes);
                    pix.Dispose();
                }
            }

            Page = wb;
        }

        [RelayCommand()]
        private void GoToNextPage()
        {
            CurrentPage++;
        }

        [RelayCommand()]
        private void GoToPreviousPage()
        {
            CurrentPage--;

        }

        [RelayCommand]
        private void GoToSection(SectionDto section)
        {
            CurrentPage = section.StartPage;
        }

        [RelayCommand]
        private void GoToBookmark(BookmarkDto bookmark)
        {
            CurrentPage = bookmark.PageNumber;
        }

        [RelayCommand]
        private async Task SaveBookmarkAsync()
        {
            var bookmark = await _bookmarkService.AddAsync(new BookmarkCreateDto { 
                Note = NewBookmarkNote, 
                PageNumber=CurrentPage,
                UserId = _auth.CurrentUserId,
                BookId = UserBook.BookId
            });
            Bookmarks.Add(bookmark);
        }

        [RelayCommand]
        private async Task DeleteBookmarkAsync(BookmarkDto bookmark)
        {
            await _bookmarkService.DeleteAsync(bookmark.BookmarkId);
            Bookmarks.Remove(bookmark);
            Console.WriteLine("hello");
        }
    }
}
