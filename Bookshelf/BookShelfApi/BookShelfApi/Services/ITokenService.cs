using BookShelfApi.Models;

namespace BookShelfApi.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
