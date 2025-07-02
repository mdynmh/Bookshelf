using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookshelf.Dtos;

namespace Bookshelf.Services
{
    public class UserService
    {
        private readonly ApiService _api;

        public UserService(ApiService apiService)
        {
            _api = apiService;
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            return await _api.GetAsync<UserDto>($"/api/users/{userId}");
        }
    }
}
