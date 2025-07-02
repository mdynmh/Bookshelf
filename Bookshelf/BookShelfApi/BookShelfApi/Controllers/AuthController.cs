using System.Text;
using BookShelfApi.DataContext;
using BookShelfApi.Models;
using BookShelfApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShelfApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly BookshelfDbContext _context;

        public AuthController(ITokenService tokenService, BookshelfDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Login == request.Login);

            if (user == null)
                return Unauthorized("Пользователь не найден");

            if (!VerifyPassword(request.Password, user.Salt, user.PasswordHash))
                return Unauthorized("Неверный пароль");

            var token = _tokenService.GenerateJwtToken(user);
            return Ok(new { token });
        }

        private bool VerifyPassword(string password, string salt, string storedHash)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var saltedPassword = salt + password;
            var computedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            var computedHashString = BitConverter.ToString(computedHash).Replace("-", "").ToLower();
            return computedHashString == storedHash.ToLower();
        }
    }
}
