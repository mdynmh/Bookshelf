using System.Security.Claims;
using BookShelfApi.DataContext;
using BookShelfApi.Dtos;
using BookShelfApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShelfApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserBooksController : ControllerBase
    {
        private readonly BookshelfDbContext _context;

        public UserBooksController(BookshelfDbContext context)
        {
            _context = context;
        }

        // GET: api/UserBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBookDto>>> GetUserBooks()
        {
            var list = await _context.UserBooks
                .Include(ub => ub.Book)
                .Include(ub => ub.Bookmarks)
                .Select(ub => new UserBookDto
                {
                    UserId = ub.UserId,
                    BookId = ub.BookId,
                    CurrentPage = ub.CurrentPage,

                    Book = new BookDto
                    {
                        BookId = ub.Book.BookId,
                        Title = ub.Book.Title,
                        Description = ub.Book.Description,
                        ImageUrl = ub.Book.ImageUrl,
                        FileUrl = ub.Book.FileUrl,
                        TotalCopies = ub.Book.TotalCopies,
                        PageCount = ub.Book.PageCount,
                        ReadingTime = ub.Book.ReadingTime,

                        Authors = ub.Book.Authors
                            .Select(a => new AuthorDto
                            {
                                AuthorId = a.AuthorId,
                                FirstName = a.FirstName,
                                LastName = a.LastName,
                                Patronymic = a.Patronymic
                            }).ToList(),
                        Genres = ub.Book.Genres
                            .Select(g => new GenreDto
                            {
                                GenreId = g.GenreId,
                                GenreName = g.GenreName
                            }).ToList(),
                        Sections = ub.Book.Sections
                            .Select(s => new SectionDto
                            {
                                SectionId = s.SectionId,
                                Title = s.Title,
                                StartPage = s.StartPage
                            }).ToList()
                    },

                    Bookmarks = ub.Bookmarks
                        .Select(bm => new BookmarkDto
                        {
                            BookmarkId = bm.BookmarkId,
                            PageNumber = bm.PageNumber,
                            Note = bm.Note
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/UserBooks/user/{userId}/book/{bookId}
        [HttpGet("user/{userId}/book/{bookId}")]
        public async Task<ActionResult<UserBookDto>> GetUserBook(int userId, int bookId)
        {
            var dto = await _context.UserBooks
                .Include(ub => ub.Book)
                .Include(ub => ub.Bookmarks)
                .Where(ub => ub.UserId == userId && ub.BookId == bookId)
                .Select(ub => new UserBookDto
                {
                    UserId = ub.UserId,
                    BookId = ub.BookId,
                    CurrentPage = ub.CurrentPage,

                    Book = new BookDto
                    {
                        BookId = ub.Book.BookId,
                        Title = ub.Book.Title,
                        Description = ub.Book.Description,
                        ImageUrl = ub.Book.ImageUrl,
                        FileUrl = ub.Book.FileUrl,
                        TotalCopies = ub.Book.TotalCopies,
                        PageCount = ub.Book.PageCount,
                        ReadingTime = ub.Book.ReadingTime,

                        Authors = ub.Book.Authors
                            .Select(a => new AuthorDto
                            {
                                AuthorId = a.AuthorId,
                                FirstName = a.FirstName,
                                LastName = a.LastName,
                                Patronymic = a.Patronymic
                            }).ToList(),
                        Genres = ub.Book.Genres
                            .Select(g => new GenreDto
                            {
                                GenreId = g.GenreId,
                                GenreName = g.GenreName
                            }).ToList(),
                        Sections = ub.Book.Sections
                            .Select(s => new SectionDto
                            {
                                SectionId = s.SectionId,
                                Title = s.Title,
                                StartPage = s.StartPage
                            }).ToList()
                    },

                    Bookmarks = ub.Bookmarks
                        .Select(bm => new BookmarkDto
                        {
                            BookmarkId = bm.BookmarkId,
                            PageNumber = bm.PageNumber,
                            Note = bm.Note
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // GET /api/userbooks/my
        [HttpGet("my"), Authorize]
        public async Task<ActionResult<List<UserBookDto>>> GetMyUserBooks()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var list = await _context.UserBooks
                .Where(ub => ub.UserId == userId)
                .Include(ub => ub.Book).ThenInclude(b => b.Authors)
                .Include(ub => ub.Book).ThenInclude(b => b.Genres)
                .Include(ub => ub.Book).ThenInclude(b => b.Sections)
                .Include(ub => ub.Bookmarks)
                .Select(ub => new UserBookDto
                {
                    UserId = ub.UserId,
                    BookId = ub.BookId,
                    CurrentPage = ub.CurrentPage,
                    Book = new BookDto
                    {
                        BookId = ub.Book.BookId,
                        Title = ub.Book.Title,
                        Description = ub.Book.Description,
                        ImageUrl = ub.Book.ImageUrl,
                        FileUrl = ub.Book.FileUrl,
                        TotalCopies = ub.Book.TotalCopies,
                        PageCount = ub.Book.PageCount,
                        ReadingTime = ub.Book.ReadingTime,
                        Authors = ub.Book.Authors.Select(a => new AuthorDto
                        {
                            AuthorId = a.AuthorId,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            Patronymic = a.Patronymic
                        }).ToList(),
                        Genres = ub.Book.Genres.Select(g => new GenreDto
                        {
                            GenreId = g.GenreId,
                            GenreName = g.GenreName
                        }).ToList(),
                        Sections = ub.Book.Sections.Select(s => new SectionDto
                        {
                            SectionId = s.SectionId,
                            Title = s.Title,
                            StartPage = s.StartPage
                        }).ToList()
                    },
                    Bookmarks = ub.Bookmarks.Select(bm => new BookmarkDto
                    {
                        BookmarkId = bm.BookmarkId,
                        PageNumber = bm.PageNumber,
                        Note = bm.Note
                    }).ToList()
                })
                .ToListAsync();

            return Ok(list);
        }

        // PUT: api/UserBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("user/{userId}/book/{bookId}")]
        public async Task<IActionResult> PutUserBook(int userId, int bookId, UserBookDto userBookDto)
        {
            if (userId != userBookDto.UserId || bookId != userBookDto.BookId)
            {
                return BadRequest();
            }

            var userBook = new UserBook
            {
                UserId = userBookDto.UserId,
                BookId = userBookDto.BookId,
                CurrentPage = userBookDto.CurrentPage
            };

            _context.Entry(userBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserBookExists(userId, bookId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserBook>> PostUserBook(UserBookDto dto)
        {
            var entity = new UserBook
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                CurrentPage = dto.CurrentPage
            };

            if (_context.UserBooks.Any(ub =>
                ub.UserId == entity.UserId && ub.BookId == entity.BookId))
            {
                return Conflict();
            }

            _context.UserBooks.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUserBook),
                new { userId = entity.UserId, bookId = entity.BookId },
                entity
            );
        }

        // DELETE: api/UserBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserBook(int id)
        {
            var userBook = await _context.UserBooks.FindAsync(id);
            if (userBook == null)
            {
                return NotFound();
            }

            _context.UserBooks.Remove(userBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserBookExists(int userId, int bookId)
        {
            return _context.UserBooks.Any(e => e.UserId == userId && e.BookId == bookId);
        }
    }
}
