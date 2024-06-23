using Sieve.Attributes;

namespace Bookstore_Management__PROJECT1.Models.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        [Sieve(CanFilter = true, CanSort = true)] public string Email { get; set; }

    }
}