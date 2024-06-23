using Bookstore_Management___Project2_.Repos;
using Bookstore_Management___Project2_.Repos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Add this using directive
using Sieve.Models;
using Sieve.Services;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Bookstore_Management___Project2_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        //inject dbcontext in controller- create a constructor
        private readonly bookstoredbcontext customerdbcontext;
        private readonly SieveProcessor sieveProcessor;

        public CustomerController(bookstoredbcontext customerdbcontext, SieveProcessor sieveProcessor)
        {
            this.customerdbcontext = customerdbcontext;
            this.sieveProcessor = sieveProcessor;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers([FromQuery] SieveModel sieveModel)
        {
            var customers = customerdbcontext.Customers.AsQueryable();
            customers = sieveProcessor.Apply(sieveModel, customers);

            return await customers.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await customerdbcontext.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }
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
        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if a customer with the same email already exists
            var existingCustomer = await customerdbcontext.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);
            if (existingCustomer != null)
            {
                ModelState.AddModelError("Email", "A customer with this email already exists.");
                return BadRequest(ModelState);
            }

            customerdbcontext.Customers.Add(customer);
            await customerdbcontext.SaveChangesAsync();

            // Return the created customer with the generated CustomerId
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }




        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await customerdbcontext.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            customerdbcontext.Customers.Remove(customer);
            await customerdbcontext.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return customerdbcontext.Customers.Any(e => e.CustomerId == id);
        }
    }
}
