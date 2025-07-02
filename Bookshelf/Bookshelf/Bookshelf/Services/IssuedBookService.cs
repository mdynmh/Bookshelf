using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookshelf.Dtos;

namespace Bookshelf.Services
{
    public class IssuedBookService
    {
        private readonly ApiService _api;

        public IssuedBookService(ApiService apiService)
        {
            _api = apiService;
        }

        public async Task<List<IssuedBookDto>> GetAllIssuedBooksAsync()
        {
            return await _api.GetAsync<List<IssuedBookDto>>("/api/issuedbooks");
        }

        public Task<List<IssuedBookDto>> GetMyIssuedBooksAsync()
        {
            return _api.GetAsync<List<IssuedBookDto>>("/api/issuedbooks/my");
        }

        public async Task AddIssuedBookAsync(IssuedBookDto newIssuedBook)
        {
            await _api.PostAsync<IssuedBookDto, IssuedBookDto>("/api/issuedbooks", newIssuedBook);
        }
    }
}
