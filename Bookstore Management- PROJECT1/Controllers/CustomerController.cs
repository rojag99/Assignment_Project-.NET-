using Bookstore_Management__PROJECT1.Data;
using Bookstore_Management__PROJECT1.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Bookstore_Management__PROJECT1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly BookstoreDbContext customerdbcontext;
        private readonly SieveProcessor sieveProcessor;

        public CustomerController(BookstoreDbContext customerdbcontext, SieveProcessor sieveProcessor)
        {
            this.customerdbcontext = customerdbcontext;
            this.sieveProcessor = sieveProcessor;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers([FromQuery] SieveModel sieveModel)
        {
            var customers = customerdbcontext.Customer.AsQueryable();
            customers = sieveProcessor.Apply(sieveModel, customers);

            return await customers.ToListAsync();
        }

        // GET /api/customers/{id} - Get customer by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await customerdbcontext.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // POST /api/customers - Create a new customer
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if a customer with the same email already exists
            var existingCustomer = await customerdbcontext.Customer.FirstOrDefaultAsync(c => c.Email == customer.Email);
            if (existingCustomer != null)
            {
                ModelState.AddModelError("Email", "A customer with this email already exists.");
                return BadRequest(ModelState);
            }

            customerdbcontext.Customer.Add(customer);
            await customerdbcontext.SaveChangesAsync();

            // Return the created customer with the generated CustomerId
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }





        // PUT /api/customers/{id} - Update a customer
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            customerdbcontext.Entry(customer).State = EntityState.Modified;

            try
            {
                await customerdbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // DELETE /api/customers/{id} - Delete a customer
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await customerdbcontext.Customer.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                customerdbcontext.Customer.Remove(customer);
                await customerdbcontext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the customer.");
            }
        }

        private bool CustomerExists(int id)
        {
            return customerdbcontext.Customer.Any(e => e.CustomerId == id);
        }
    }
}
    
