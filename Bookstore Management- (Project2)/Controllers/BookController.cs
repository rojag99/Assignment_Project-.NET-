using Bookstore_Management___Project2_.Repos.Models;
using Bookstore_Management___Project2_.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using Sieve.Models;

namespace Bookstore_Management___Project2_.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly bookstoredbcontext bookdbcontext;
        private readonly SieveProcessor sieveProcessor;

        public BookController(bookstoredbcontext bookdbcontext,SieveProcessor sieveProcessor)
        {
            this.bookdbcontext = bookdbcontext;
            this.sieveProcessor = sieveProcessor;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks([FromQuery] SieveModel sieveModel)
        {
            var books = bookdbcontext.Books.AsQueryable();
            books = sieveProcessor.Apply(sieveModel, books);

            return await books.ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Book>> Get(int id)
        {
            var book = await bookdbcontext.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(); // Return 404 if book is not found
            }

            return book; // Return book if found
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bookdbcontext.Books.Add(book);
            await bookdbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = book.BookId }, book);
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
        [ProducesResponseType(204)] // No Content (Success)
        [ProducesResponseType(404)] // Not Found
        [ProducesResponseType(400)] // Bad Request (if ID format is incorrect, for example)
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await bookdbcontext.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(); // 404 if book not found
            }

            bookdbcontext.Books.Remove(book);
            await bookdbcontext.SaveChangesAsync();

            return NoContent(); // 204 (Success, no content)
        }
    }
}