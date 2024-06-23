using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace Bookstore_Management___Project2_.Repos.Models;

[Table("Customer")]
public partial class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [StringLength(100)]
    [Sieve(CanFilter=true, CanSort=true)]public string Name { get; set; }

    [StringLength(100)]
    public string Email { get; set; }
}
