using Bookstore_Management___Project2_.Repos.Models;
using Bookstore_Management___Project2_.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using Sieve.Models;

namespace Bookstore_Management___Project2_.Controllers.v2
{
    [Route("api/v2{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly bookstoredbcontext bookdbcontext;
        private readonly SieveProcessor sieveProcessor;

        public BookController(SieveProcessor sieveProcessor)
        {
            this.sieveProcessor = sieveProcessor;
        }

        public BookController(bookstoredbcontext bookdbcontext)
        {
            this.bookdbcontext = bookdbcontext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks([FromQuery] SieveModel sieveModel)
        {
            var books = bookdbcontext.Books.AsQueryable();
            books = sieveProcessor.Apply(sieveModel, books);

            return await books.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(int id)  // Changed method name to 'Get'
        {
            var book = await bookdbcontext.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            bookdbcontext.Books.Add(book);
            await bookdbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = book.BookId }, book);  // Changed action name to 'Get'
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            bookdbcontext.Entry(book).State = EntityState.Modified;

            try
            {
                await bookdbcontext.SaveChangesAsync();
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

        private bool BookExists(int id)
        {
            return bookdbcontext.Books.Any(e => e.BookId == id);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await bookdbcontext.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            bookdbcontext.Books.Remove(book);
            await bookdbcontext.SaveChangesAsync();

            return NoContent();
        }
    }
}