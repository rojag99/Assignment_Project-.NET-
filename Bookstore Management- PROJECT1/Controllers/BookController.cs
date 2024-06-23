using Bookstore_Management__PROJECT1.Data;
using Bookstore_Management__PROJECT1.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.Json;
using Sieve.Services;
using Sieve.Models;
using System;


namespace Bookstore_Management__PROJECT1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookstoreDbContext bookdbcontext;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly SieveProcessor sieveProcessor;

        public BookController(BookstoreDbContext bookdbcontext, SieveProcessor sieveProcessor)
        {
            this.bookdbcontext = bookdbcontext;
            this.sieveProcessor = sieveProcessor;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true // Optional: for pretty-printing JSON output
            };

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Entities.Book>>> GetBooks([FromQuery] SieveModel sieveModel)
        {
            var booksQuery = bookdbcontext.Book.Include(b => b.Author).AsQueryable();
            booksQuery = sieveProcessor.Apply(sieveModel, booksQuery);

            var books = await booksQuery.ToListAsync();
            var jsonBooks = JsonSerializer.Serialize(books, _jsonOptions);
            return Content(jsonBooks, "application/json");
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Models.Entities.Book), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Models.Entities.Book>> GetBook(int id)
        {
            var book = await bookdbcontext.Book.Include(b => b.Author).FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound(); // 404 if book not found
            }

            var jsonBook = JsonSerializer.Serialize(book, _jsonOptions);
            return Content(jsonBook, "application/json"); // 200 with JSON content
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            bookdbcontext.Book.Add(book);
            await bookdbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(Book), new { id = book.BookId }, book);  // Changed action name to 'Get'
        }
        // PUT /api/books/{id} - Update a book
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Models.Entities.Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            var existingBook = await bookdbcontext.Book.FindAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = book.Title;
            existingBook.Genre = book.Genre;
            existingBook.PublicationDate = book.PublicationDate;
            existingBook.AuthorId = book.AuthorId;



            return NoContent();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(204)] // No Content (Success)
        [ProducesResponseType(404)] // Not Found
        [ProducesResponseType(400)] // Bad Request
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await bookdbcontext.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound(); // 404 if book not found
            }

            bookdbcontext.Book.Remove(book);
            await bookdbcontext.SaveChangesAsync();

            return NoContent(); // 204 (Success, no content)
        }


    }
}











