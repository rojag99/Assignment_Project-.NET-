using Bookstore_Management__PROJECT1.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_Management__PROJECT1.Data
{
    public class BookstoreDbContext : DbContext
    {
        public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options)

        {

        }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Customer> Customer { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Book Entity Configuration
            modelBuilder.Entity<Book>()
            .HasKey(b => b.BookId);

            modelBuilder.Entity<Book>()
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Book>()
                .Property(b => b.Genre)
                .HasMaxLength(100);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            // Author configuration
            modelBuilder.Entity<Author>()
                .HasKey(a => a.AuthorId);

            modelBuilder.Entity<Author>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Customer configuration
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerId);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            base.OnModelCreating(modelBuilder);
        }
    }
}

   