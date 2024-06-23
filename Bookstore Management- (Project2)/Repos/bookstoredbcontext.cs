using System;
using System.Collections.Generic;
using Bookstore_Management___Project2_.Repos.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_Management___Project2_.Repos;

public partial class bookstoredbcontext : DbContext
{
    public bookstoredbcontext()
    {
    }

    public bookstoredbcontext(DbContextOptions<bookstoredbcontext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-9GGG85E\\SQLEXPRESS; User id=rojagserver;Password=roja-1234; Initial Catalog= bookstoremanagement;Integrated Security=false; Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    //}

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
        // Fluent API configuration for Customer entity
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerId")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .HasMaxLength(100);
        });

        // Fluent API configuration for Author entity
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId);

            entity.Property(e => e.AuthorId)
                .HasColumnName("AuthorId")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasMany(e => e.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Fluent API configuration for Book entity
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId);

            entity.Property(e => e.BookId)
                .HasColumnName("BookId")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Genre)
                .HasMaxLength(50);

            entity.Property(e => e.PublicationDate)
                .HasColumnType("datetime");

            entity.HasIndex(e => e.AuthorId)
                .HasDatabaseName("IX_Book_AuthorId");

            entity.HasOne(d => d.Author)
                .WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Book_Author");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
}
