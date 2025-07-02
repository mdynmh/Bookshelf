using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Bookshelf.Dtos;

namespace Bookshelf.Services
{
    public class AuthService
    {
        private ApiService _api;

        public int CurrentUserId;

        public AuthService(ApiService apiService)
        {
            _api = apiService;
        }

        public async Task LoginAsync(string login, string password)
        {
            var req = new 
            {
                Login = login,
                Password = password
            };

            var resp = await _api.PostAsync<object, JsonDocument>(
                "/api/auth/login",
                req
            );

            var token = resp.RootElement.GetProperty("token").ToString();

            _api.SetAuthToken(token);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var subClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            if (subClaim != null && int.TryParse(subClaim.Value, out var id))
                CurrentUserId = id;
        }
    }
}
