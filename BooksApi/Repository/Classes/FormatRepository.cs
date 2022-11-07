using BooksApi.Data;
using BooksApi.Models;
using BooksApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Repository.Classes {
    public class FormatRepository : IFormatRepository {
        private readonly AppDbContext _context;
        public FormatRepository(AppDbContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Format>> GetFormats(CancellationToken token) {
            return await _context.Formats.ToListAsync(token);
        }

        public async Task<Format?> GetFormatById(int? id, CancellationToken token) {
            return await _context.Formats.FirstOrDefaultAsync(e => e.Id == id, token);

        }

        public async Task<Format?> UpdateFormat(Format format, CancellationToken token) {
            Format? oldFormat = await GetFormatById(format.Id, token);
            if(oldFormat is null) {
                return null;
            }

            try
            {
                oldFormat.Name = format.Name;
                oldFormat.UpdatedAt = DateTime.Today;
                await _context.SaveChangesAsync(token);
            }
            catch (Exception)
            {
                throw;
            }
            return  oldFormat;
        }

        public async Task<Format?> CreateFormat(Format? format, CancellationToken token) {
            if(format is null) {
                return null;
            }
            var newFormat = await _context.Formats.AddAsync(format, token);
            await _context.SaveChangesAsync(token);
            return newFormat.Entity;
        }

        public async Task<bool> DeleteFormat(int id, CancellationToken token) {
            var format = await GetFormatById(id, token);
            if(format is null) {
                return false;
            }
            _context.Formats.Remove(format);
            await _context.SaveChangesAsync(token);
            return true;
        }
    }
}