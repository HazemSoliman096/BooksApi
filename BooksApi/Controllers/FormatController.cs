using Microsoft.AspNetCore.Mvc;
using BooksApi.Repository.Interfaces;
using BooksApi.Models;
using BooksApi.Repository.Classes;

namespace BooksApi.Controllers {
    [ApiController]
    [Route("api/format")]
    public class FormatController : ControllerBase {
        private readonly IFormatRepository _formatRepository;
        public FormatController(IFormatRepository formatRepository) {
            _formatRepository = formatRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Format>>> GetFormats(CancellationToken cancellationToken) {
            var formats = await _formatRepository.GetFormats(cancellationToken);
            return Ok(formats);
        }

        [HttpPost]
        public async Task<ActionResult<Format>> AddFormat([Bind("Name, CreatedAt, UpdatedAt")] Format format,
            CancellationToken token)
        {
            ModelState.Remove("Books");
            var newFormat = await _formatRepository.CreateFormat(format, token);
            if(newFormat != null)
            {
                return CreatedAtAction(nameof(GetFormats), new { id = newFormat.Id }, newFormat);
            } else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Can't create format check input values and retry.");
            }
        }

        [HttpGet("{id}")]
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

            return Ok(format);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id, CancellationToken token)
        {
            var author = await _formatRepository.GetFormatById(id, token);

            if (author == null)
            {
                return NotFound();
            }

            await _formatRepository.DeleteFormat(id, token);
            return NoContent();
        }

        [HttpPut("{id}/{name}")]
        public async Task<IActionResult> EditFormat(int id, string name, CancellationToken token)
        {

            try
            {
                await _formatRepository.UpdateFormat(id, name, token);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Can't update format check input values and retry.\n " + e.Message);
            }

            return NoContent();
        }
    }
}