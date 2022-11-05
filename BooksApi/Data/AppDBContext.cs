using Microsoft.EntityFrameworkCore;
using BooksApi.Models;

namespace BooksApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Format> Formats { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(b =>
            {
                b.HasOne(x => x.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(x => x.AuthorId);

                b.HasOne(x => x.Format)
                    .WithMany(a => a.Books)
                    .HasForeignKey(x => x.FormatId);

                b.HasOne(x => x.Genre)
                    .WithMany(a => a.Books)
                    .HasForeignKey(x => x.GenreId);
            });

            modelBuilder.Entity<Author>(b =>
            {
                b.HasMany(x => x.Books)
                    .WithOne(a => a.Author);
            });

            modelBuilder.Entity<Format>(b =>
            {
                b.HasMany(x => x.Books)
                    .WithOne(a => a.Format);
            });

            modelBuilder.Entity<Genre>(b =>
            {
                b.HasMany(x => x.Books)
                    .WithOne(a => a.Genre);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}