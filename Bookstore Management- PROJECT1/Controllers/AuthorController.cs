using Bookstore_Management__PROJECT1.Data;
using Bookstore_Management__PROJECT1.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_Management__PROJECT1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly BookstoreDbContext authordbcontext;

        public AuthorController(BookstoreDbContext authordbcontext)
        {
            this.authordbcontext = authordbcontext;
        }


        [HttpGet]
        // Specify the API version here
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await authordbcontext.Author.Include(a => a.Books).ToListAsync();
        }


        // GET /api/authors/{id} - Get author by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await authordbcontext.Author.Include(a => a.Books).FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // POST /api/authors - Create a new author
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            authordbcontext.Author.Add(author);
            await authordbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorId }, author);
        }
        // PUT /api/authors/{id} - Update an author
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, Author author)
        {
            if (id != author.AuthorId)
            {
                return BadRequest();
            }

            authordbcontext.Entry(author).State = EntityState.Modified;

            try
            {
                await authordbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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
        private bool AuthorExists(int id)
        {
            return authordbcontext.Author.Any(e => e.AuthorId == id);
        }
        // DELETE /api/authors/{id} - Delete an author
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await authordbcontext.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            authordbcontext.Author.Remove(author);
            await authordbcontext.SaveChangesAsync();
            return NoContent();
        }

    }
}
       