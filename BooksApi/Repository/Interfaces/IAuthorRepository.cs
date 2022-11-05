using BooksApi.Models;

namespace BooksApi.Repository.Interfaces {
    public interface IAuthorRepository {
        public Task<ICollection<Author>> GetAuthors(CancellationToken token);
        public Task<Author?> GetAuthorById(int id, CancellationToken token);
        public Task<Author?> UpdateAuthor(Author author, CancellationToken token);
        public Task<Author?> CreateAuthor(Author author, CancellationToken token);
        public Task<bool> DeleteAuthor(int id, CancellationToken token);
    }
}