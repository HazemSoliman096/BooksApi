using BooksApi.Models;

namespace BooksApi.Repository.Interfaces {
    public interface IFormatRepository {
        public Task<ICollection<Format>> GetFormats(CancellationToken token);
        public Task<Format?> GetFormatById(int? id, CancellationToken token);
        public Task<Format?> UpdateFormat(int id, string name, CancellationToken token);
        public Task<Format?> CreateFormat(Format format, CancellationToken token);
        public Task<bool> DeleteFormat(int id, CancellationToken token);
    }
}