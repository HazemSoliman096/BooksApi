using BooksApi.Models;
using BooksApi.Repository.Interfaces;
using BooksApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Repository.Classes {
    public class AuthorRepository : IAuthorRepository {
        private readonly AppDbContext _context;
        public AuthorRepository(AppDbContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Author>> GetAuthors(CancellationToken token) {
            return await _context.Authors.ToListAsync(token);
        }

        public async Task<Author?> GetAuthorById(int id, CancellationToken token) {
            return await _context.Authors.FirstOrDefaultAsync(e => e.Id == id, token);

        }

        public async Task<Author?> UpdateAuthor(Author author, CancellationToken token) {
            Author? oldAuthor = await GetAuthorById(author.Id, token);
            if(oldAuthor is null) {
                return null;
            }

            try
            {
                oldAuthor.Name = author.Name;
                oldAuthor.UpdatedAt = DateTime.Today;
                await _context.SaveChangesAsync(token);
            }
            catch (Exception e)
            {
                throw;
            }
            return  oldAuthor;
        }

        public async Task<Author?> CreateAuthor(Author? author, CancellationToken token) {
            if(author is null) {
                return null;
            }
            var newAuthor = await _context.Authors.AddAsync(author, token);
            await _context.SaveChangesAsync(token);
            return newAuthor.Entity;
        }

        public async Task<bool> DeleteAuthor(int id, CancellationToken token) {
            var author = await GetAuthorById(id, token);
            if(author is null) {
                return false;
            }
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync(token);
            return true;
        }
    }
}