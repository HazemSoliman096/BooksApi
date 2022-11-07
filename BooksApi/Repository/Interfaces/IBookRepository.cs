using BooksApi.Models;

namespace BooksApi.Repository.Interfaces {
    public interface IBookRepository {
        public Task<ICollection<Book>> GetBooks(CancellationToken token);
        public Task<Book?> GetBookById(int? id, CancellationToken token);
        public Task<Book?> UpdateBook(int id, Book book, CancellationToken token);
        public Task<Book?> CreateBook(Book book, CancellationToken token);
        public Task<bool> DeleteBook(int id, CancellationToken token);
    }
}