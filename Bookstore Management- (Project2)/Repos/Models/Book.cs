using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace Bookstore_Management___Project2_.Repos.Models;

[Table("Book")]
[Index("AuthorId", Name = "IX_Book_AuthorId")]
public partial class Book
{
    [Key]
    public int BookId { get; set; }

    [StringLength(200)]
    public string Title { get; set; }

    [StringLength(50)]
    public string Genre { get; set; } 

    public DateTime PublicationDate { get; set; }

    [Sieve(CanFilter = true, CanSort = true)] public int AuthorId { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Books")]
    public virtual Author Author { get; set; } 
}
