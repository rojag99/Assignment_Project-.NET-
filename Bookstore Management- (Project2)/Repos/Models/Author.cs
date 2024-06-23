using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_Management___Project2_.Repos.Models;

[Table("Author")]
public partial class Author
{
    [Key]
    public int AuthorId { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    [InverseProperty("Author")]
    public virtual ICollection<Book> Books { get; set; }
}
