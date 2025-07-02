using System.Collections.Generic;
using System.Threading.Tasks;
using Bookshelf.Dtos;

namespace Bookshelf.Services
{
    public class UserBookService
    {
        private readonly ApiService _api;

        public UserBookService(ApiService apiService)
        {
            _api = apiService;
        }

        public async Task<List<UserBookDto>> GetAllUserBooksAsync()
        {
            return await _api.GetAsync<List<UserBookDto>>("/api/userbooks");
        }

        public async Task<UserBookDto?> GetUserBookAsync(int userId, int bookId)
        {
            try
            {
                return await _api.GetAsync<UserBookDto>($"/api/userbooks/user/{userId}/book/{bookId}");
            }
            catch
            {
                return null;
            }
        }

        public Task<List<UserBookDto>> GetMyUserBooksAsync()
        {
            return _api.GetAsync<List<UserBookDto>>("/api/userbooks/my");
        }

        public async Task<UserBookDto> AddUserBookAsync(UserBookCreateDto newUserBook)
        {
            return await _api.PostAsync<UserBookCreateDto, UserBookDto>("/api/userbooks", newUserBook);
        }

        public async Task UpdateIssuedBookAsync(UserBookDto updatedUserBook)
        {
            await _api.PutAsync<UserBookDto, UserBookDto>($"/api/userbooks/user/{updatedUserBook.UserId}/book/{updatedUserBook.BookId}",
                updatedUserBook);
        }
    }
}
