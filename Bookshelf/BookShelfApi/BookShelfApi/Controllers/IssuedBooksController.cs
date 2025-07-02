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
    public class IssuedBooksController : ControllerBase
    {
        private readonly BookshelfDbContext _context;

        public IssuedBooksController(BookshelfDbContext context)
        {
            _context = context;
        }

        // GET: api/IssuedBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IssuedBook>>> GetIssuedBooks()
        {
            var list = await _context.IssuedBooks
            .Include(ib => ib.Book)
            .Select(ib => new IssuedBookDto
            {
                IssueId = ib.IssueId,
                UserId = ib.UserId,
                BookId = ib.BookId,
                IssuedAt = ib.IssuedAt,
                DueDate = ib.DueDate,
                ReturnedAt = ib.ReturnedAt,

                Book = new BookDto
                {
                    BookId = ib.Book.BookId,
                    Title = ib.Book.Title,
                    Description = ib.Book.Description,
                    ImageUrl = ib.Book.ImageUrl,
                    FileUrl = ib.Book.FileUrl,
                    TotalCopies = ib.Book.TotalCopies,
                    PageCount = ib.Book.PageCount,
                    ReadingTime = ib.Book.ReadingTime,

                    Authors = ib.Book.Authors
                        .Select(a => new AuthorDto
                        {
                            AuthorId = a.AuthorId,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            Patronymic = a.Patronymic
                        }).ToList(),
                    Genres = ib.Book.Genres
                        .Select(g => new GenreDto
                        {
                            GenreId = g.GenreId,
                            GenreName = g.GenreName
                        }).ToList(),
                    Sections = ib.Book.Sections
                        .Select(s => new SectionDto
                        {
                            SectionId = s.SectionId,
                            Title = s.Title,
                            StartPage = s.StartPage
                        }).ToList()
                }
            })
            .ToListAsync();

            return Ok(list);
        }

        // GET: api/IssuedBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IssuedBook>> GetIssuedBook(int id)
        {
            var dto = await _context.IssuedBooks
            .Include(ib => ib.Book)
            .Where(ib => ib.IssueId == id)
            .Select(ib => new IssuedBookDto
            {
                IssueId = ib.IssueId,
                UserId = ib.UserId,
                BookId = ib.BookId,
                IssuedAt = ib.IssuedAt,
                DueDate = ib.DueDate,
                ReturnedAt = ib.ReturnedAt,

                Book = new BookDto
                {
                    BookId = ib.Book.BookId,
                    Title = ib.Book.Title,
                    Description = ib.Book.Description,
                    ImageUrl = ib.Book.ImageUrl,
                    FileUrl = ib.Book.FileUrl,
                    TotalCopies = ib.Book.TotalCopies,
                    PageCount = ib.Book.PageCount,
                    ReadingTime = ib.Book.ReadingTime,

                    Authors = ib.Book.Authors
                        .Select(a => new AuthorDto
                        {
                            AuthorId = a.AuthorId,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            Patronymic = a.Patronymic
                        }).ToList(),
                    Genres = ib.Book.Genres
                        .Select(g => new GenreDto
                        {
                            GenreId = g.GenreId,
                            GenreName = g.GenreName
                        }).ToList(),
                    Sections = ib.Book.Sections
                        .Select(s => new SectionDto
                        {
                            SectionId = s.SectionId,
                            Title = s.Title,
                            StartPage = s.StartPage
                        }).ToList()
                }
            })
            .FirstOrDefaultAsync();

            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // GET /api/issuedbooks/my
        [HttpGet("my"), Authorize]
        public async Task<ActionResult<List<IssuedBookDto>>> GetMyIssuedBooks()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var list = await _context.IssuedBooks
                .Where(ib => ib.UserId == userId)
                .Include(ib => ib.Book).ThenInclude(b => b.Authors)
                .Include(ib => ib.Book).ThenInclude(b => b.Genres)
                .Include(ib => ib.Book).ThenInclude(b => b.Sections)
                .Select(ib => new IssuedBookDto
                {
                    IssueId = ib.IssueId,
                    UserId = ib.UserId,
                    BookId = ib.BookId,
                    IssuedAt = ib.IssuedAt,
                    DueDate = ib.DueDate,
                    ReturnedAt = ib.ReturnedAt,
                    Book = new BookDto
                    {
                        BookId = ib.Book.BookId,
                        Title = ib.Book.Title,
                        Description = ib.Book.Description,
                        ImageUrl = ib.Book.ImageUrl,
                        FileUrl = ib.Book.FileUrl,
                        TotalCopies = ib.Book.TotalCopies,
                        PageCount = ib.Book.PageCount,
                        ReadingTime = ib.Book.ReadingTime,
                        Authors = ib.Book.Authors.Select(a => new AuthorDto
                        {
                            AuthorId = a.AuthorId,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            Patronymic = a.Patronymic
                        }).ToList(),
                        Genres = ib.Book.Genres.Select(g => new GenreDto
                        {
                            GenreId = g.GenreId,
                            GenreName = g.GenreName
                        }).ToList(),
                        Sections = ib.Book.Sections.Select(s => new SectionDto
                        {
                            SectionId = s.SectionId,
                            Title = s.Title,
                            StartPage = s.StartPage
                        }).ToList()
                    }
                })
                .ToListAsync();

            return Ok(list);
        }

        // PUT: api/IssuedBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "administrator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssuedBook(int id, IssuedBook issuedBook)
        {
            if (id != issuedBook.IssueId)
            {
                return BadRequest();
            }

            _context.Entry(issuedBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssuedBookExists(id))
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

        // POST: api/IssuedBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "administrator")]
        [HttpPost]
        public async Task<ActionResult<IssuedBook>> PostIssuedBook(IssuedBook issuedBook)
        {
            _context.IssuedBooks.Add(issuedBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssuedBook", new { id = issuedBook.IssueId }, issuedBook);
        }

        // DELETE: api/IssuedBooks/5
        [Authorize(Roles = "administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssuedBook(int id)
        {
            var issuedBook = await _context.IssuedBooks.FindAsync(id);
            if (issuedBook == null)
            {
                return NotFound();
            }

            _context.IssuedBooks.Remove(issuedBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IssuedBookExists(int id)
        {
            return _context.IssuedBooks.Any(e => e.IssueId == id);
        }
    }
}
