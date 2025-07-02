using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.Services
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task DeleteAsync(string endpoint);
        void SetAuthToken(string accessToken);
    }
}
