using BooksApi.Data;
using BooksApi.Models;
using BooksApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Repository.Classes {
    public class GenreRepository : IGenreRepository {
        private readonly AppDbContext _context;
        public GenreRepository(AppDbContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Genre>> GetGenres(CancellationToken token) {
            return await _context.Genres.ToListAsync(token);
        }

        public async Task<Genre?> GetGenreById(int? id, CancellationToken token) {
            return await _context.Genres.FirstOrDefaultAsync(e => e.Id == id, token);

        }

        public async Task<Genre?> UpdateGenre(Genre genre, CancellationToken token) {
            Genre? oldGenre = await GetGenreById(genre.Id, token);
            if(oldGenre is null) {
                return null;
            }

            try
            {
                oldGenre.Name = genre.Name;
                oldGenre.UpdatedAt = DateTime.Today;
                await _context.SaveChangesAsync(token);
            }
            catch (Exception e)
            {
                throw;
            }
            return  oldGenre;
        }

        public async Task<Genre?> CreateGenre(Genre? genre, CancellationToken token) {
            if(genre is null) {
                return null;
            }
            var newGenre = await _context.Genres.AddAsync(genre, token);
            await _context.SaveChangesAsync(token);
            return newGenre.Entity;
        }

        public async Task<bool> DeleteGenre(int id, CancellationToken token) {
            var genre = await GetGenreById(id, token);
            if(genre is null) {
                return false;
            }
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync(token);
            return true;
        }
    }
}