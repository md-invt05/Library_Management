using Library_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Library_Management.Data;

public class LibraryDbContext : DbContext
{
    public DbSet<Book> Books { get; set; } =  null!;

    public DbSet<Author> Authors { get; set; } =  null!;
    
    public DbSet<Genre> Genres { get; set; } =  null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "library.db");
        optionsBuilder.UseNpgsql($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entiy =>
            {
                entiy.HasKey(b => b.Id);
                entiy.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entiy.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
                entiy.Property(b => b.PublishYear).IsRequired();
                entiy.Property(b => b.QuantitiyInStock).IsRequired();
                
                entiy.HasOne(b => b.Author) 
                     .WithMany(a => a.Books)
                     .HasForeignKey(b => b.AuthorId)
                     .OnDelete(DeleteBehavior.Cascade);
                
                entiy.HasOne(b => b.Genre)
                    .WithMany(g => g.Books)
                    .HasForeignKey(b => b.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        );

        modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.DateBirth).IsRequired();
                entity.Property(a => a.Country).IsRequired().HasMaxLength(100);
            }
        );

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Name).IsRequired().HasMaxLength(100);
            entity.Property(g => g.Description).IsRequired().HasMaxLength(250);
        });
    }
    
}