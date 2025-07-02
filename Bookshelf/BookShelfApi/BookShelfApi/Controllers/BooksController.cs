using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookShelfApi.DataContext;
using BookShelfApi.Models;
using Microsoft.AspNetCore.Authorization;
using BookShelfApi.Dtos;

namespace BookShelfApi.Controllers
{ 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookshelfDbContext _context;

        public BooksController(BookshelfDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Include(b => b.Sections)
                .Select(b => new BookDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Description = b.Description,
                    ImageUrl = b.ImageUrl,
                    FileUrl = b.FileUrl,
                    TotalCopies = b.TotalCopies,
                    PageCount = b.PageCount,
                    ReadingTime = b.ReadingTime,

                    Authors = b.Authors
                        .Select(a => new AuthorDto
                        {
                            AuthorId = a.AuthorId,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            Patronymic = a.Patronymic
                        }).ToList(),

                    Genres = b.Genres
                        .Select(g => new GenreDto
                        {
                            GenreId = g.GenreId,
                            GenreName = g.GenreName
                        }).ToList(),

                    Sections = b.Sections
                        .Select(s => new SectionDto
                        {
                            SectionId = s.SectionId,
                            Title = s.Title,
                            StartPage = s.StartPage
                        }).ToList()
                })
                .ToListAsync();

            return Ok(books);
        }


        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var dto = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Include(b => b.Sections)
                .Where(b => b.BookId == id)
                .Select(b => new BookDto { /* то же, что выше */ })
                .FirstOrDefaultAsync();

            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "administrator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "administrator")]
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [Authorize(Roles = "administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/cover")]
        public async Task<IActionResult> GetCover(int id)
        {
            var path = await _context.Books
                .Where(b => b.BookId == id)
                .Select(b => b.ImageUrl)
                .FirstOrDefaultAsync();

            return ServeFile(path);
        }

        [HttpGet("{id}/file")]
        public async Task<IActionResult> GetFile(int id)
        {
            var path = await _context.Books
                .Where(b => b.BookId == id)
                .Select(b => b.FileUrl)
                .FirstOrDefaultAsync();

            return ServeFile(path);
        }

        private IActionResult ServeFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
                return NotFound();

            return PhysicalFile(path, "application/octet-stream");
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
