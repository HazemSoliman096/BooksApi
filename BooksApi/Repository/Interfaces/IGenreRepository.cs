using BooksApi.Models;

namespace BooksApi.Repository.Interfaces {
    public interface IGenreRepository {
        public Task<ICollection<Genre>> GetGenres(CancellationToken token);
        public Task<Genre?> GetGenreById(int? id, CancellationToken token);
        public Task<Genre?> UpdateGenre(int id, string genre, CancellationToken token);
        public Task<Genre?> CreateGenre(Genre genre, CancellationToken token);
        public Task<bool> DeleteGenre(int id, CancellationToken token);
    }
}