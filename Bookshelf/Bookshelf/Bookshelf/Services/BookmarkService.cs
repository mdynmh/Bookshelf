using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookshelf.Dtos;

namespace Bookshelf.Services
{
    public sealed class BookmarkService
    {
        private readonly ApiService _api;

        public BookmarkService(ApiService apiService)
        {
            _api = apiService;
        }

        public async Task<BookmarkDto> AddAsync(BookmarkCreateDto bookmark)
        {
            var newBookmark = await _api.PostAsync<BookmarkCreateDto, BookmarkDto>(
                $"/api/bookmarks",bookmark);
            return newBookmark;
        }

        public async Task DeleteAsync(int bookmarkId)
        {
            await _api.DeleteAsync($"/api/bookmarks/{bookmarkId}");
        }
    }
}
