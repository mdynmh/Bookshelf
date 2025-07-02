using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Bookshelf.Dtos;
using System.IO;

namespace Bookshelf.Services
{
    public class BookService
    {
        private readonly ApiService _api;

        public BookService(ApiService apiService)
        {
            _api = apiService;
        }

        public async Task<List<BookDto>> GetAllBooksAsync()
        {
            return await _api.GetAsync<List<BookDto>>("/api/books");
        }

        public async Task<BookDto> GetBookByIdAsync(int bookId)
        {
            return await _api.GetAsync<BookDto>($"/api/books/{bookId}");
        }

        public async Task<string?> DownloadBookFileAsync(int bookId)
        {
            var path = GetBookFilePath(bookId);

            if (!File.Exists(path))
            {
                var success = await _api.DownloadFileAsync($"/api/books/{bookId}/file", path);
                if (!success)
                    return null;
            }

            return path;
        }

        public async Task<Bitmap?> LoadBookCoverAsync(int bookId)
        {
            var stream = await _api.GetStreamAsync($"/api/books/{bookId}/cover");
            if (stream == null)
                return null;

            return new Bitmap(stream);
        }

        private static string GetBookFilePath(int bookId)
        {
            var folder = Path.Combine(AppContext.BaseDirectory, "Downloads");
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, $"book_{bookId}.pdf");
        }
    }

}
