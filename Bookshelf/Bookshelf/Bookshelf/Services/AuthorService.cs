using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookshelf.Dtos;

namespace Bookshelf.Services
{
    public class AuthorService
    {
        private readonly ApiService _api;

        public AuthorService(ApiService apiService)
        {
            _api = apiService;
        }

        public async Task<List<AuthorDto>> GetAllAuthorsAsync()
        {
            return await _api.GetAsync<List<AuthorDto>>("/api/authors");
        }

        public async Task<AuthorDto> GetAuthorByIdAsync(int id)
        {
            return await _api.GetAsync<AuthorDto>($"/api/authors/{id}");
        }
    }

}
