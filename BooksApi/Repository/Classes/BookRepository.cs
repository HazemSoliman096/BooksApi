using BooksApi.Data;
using BooksApi.Models;
using BooksApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Repository.Classes {
  public class BookRepository : IBookRepository {
    private readonly AppDbContext _context;
    public BookRepository(AppDbContext context) {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ICollection<Book>> GetBooks(CancellationToken token) {
      return await _context.Books
                  .Include(e => e.Author)
                  .Include(e => e.Genre)
                  .Include(e => e.Format)
                  .ToListAsync(token);
    }

    public async Task<Book?> GetBookById(int id, CancellationToken token) {
      return await _context.Books
                  .Include(e => e.Author)
                  .Include(e => e.Genre)
                  .Include(e => e.Format)
                  .FirstOrDefaultAsync(e => e.Id == id, token);

    }

    public async Task<Book?> UpdateBook(Book book, CancellationToken token) {
      Book? oldbook = await GetBookById(book.Id, token);
      if(oldbook is null) {
        return null;
      }

      try
      {
        oldbook.Title = book.Title;
        oldbook.AuthorId = book.AuthorId;
        oldbook.Pages = book.Pages;
        oldbook.FormatId = book.FormatId;
        oldbook.GenreId = book.GenreId;
        oldbook.UpdatedAt = DateTime.Today;
        await _context.SaveChangesAsync(token);
      }
      catch (Exception e)
      {
        throw;
      }
      return  oldbook;
    }

    public async Task<Book?> CreateBook(Book? book, CancellationToken token) {
      if(book is null) {
        return null;
      }
      var newBook = await _context.Books.AddAsync(book, token);
      await _context.SaveChangesAsync(token);
      return newBook.Entity;
    }

    public async Task<bool> DeleteBook(int id, CancellationToken token) {
      var book = await GetBookById(id, token);
      if(book is null) {
        return false;
      }
      _context.Books.Remove(book);
      await _context.SaveChangesAsync(token);
      return true;
    }
  }
}