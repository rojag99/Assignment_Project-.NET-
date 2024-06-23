using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bookstore_Management__PROJECT1.Models.Entities
{
    using Sieve.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Book
    {
        public int BookId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(50)]
        public string Genre { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)] public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
