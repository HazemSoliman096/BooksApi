using Microsoft.AspNetCore.Mvc;
using BooksApi.Repository.Interfaces;
using BooksApi.Models;

namespace BooksApi.Controllers {
    [ApiController]
    [Route("api/format")]
    public class FormatController : ControllerBase {
        private readonly IFormatRepository _formatRepository;
        public FormatController(IFormatRepository formatRepository) {
            _formatRepository = formatRepository;
        }

        [HttpGet]
        public async Task <ICollection<Format>> GetFormats(CancellationToken cancellationToken) {
            var formats = await _formatRepository.GetFormats(cancellationToken);
            return formats;
        }

        [HttpPost]
        public async Task<ActionResult<Format>> AddFormat([Bind("Name, CreatedAt, UpdatedAt")] Format format,
            CancellationToken token)
        {
            ModelState.Remove("Books");
            var newFormat = await _formatRepository.CreateFormat(format, token);
            return CreatedAtAction(nameof(GetFormats), new {id = newFormat.Id}, newFormat);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Format>> GetFormat(int? id, CancellationToken token)
        {
            if (id == null)
            {
                return NotFound();
            }
            var format = await _formatRepository.GetFormatById(id, token);

            if (format == null)
            {
                return NotFound();
            }

            return format;
        }
    }
}