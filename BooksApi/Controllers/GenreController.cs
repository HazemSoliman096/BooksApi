using Microsoft.AspNetCore.Mvc;
using BooksApi.Repository.Interfaces;
using BooksApi.Models;
using BooksApi.Repository.Classes;

namespace BooksApi.Controllers {
    [ApiController]
    [Route("api/genre")]
    public class GenreController : ControllerBase {
        private readonly IGenreRepository _genreRepository;
        public GenreController(IGenreRepository genreRepository) {
            _genreRepository = genreRepository;
        }

        [HttpGet]
        public async Task <ActionResult<ICollection<Genre>>> GetGenres(CancellationToken cancellationToken) {
            var formats = await _genreRepository.GetGenres(cancellationToken);
            return Ok(formats);
        }

        [HttpPost]
        public async Task<ActionResult<Genre>> AddGenre([Bind("Name, CreatedAt, UpdatedAt")] Genre format,
            CancellationToken token)
        {
            ModelState.Remove("Books");
            var newGenre = await _genreRepository.CreateGenre(format, token);

            if (newGenre != null)
            {
                return CreatedAtAction(nameof(GetGenres), new { id = newGenre.Id }, newGenre);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Can't create genre check input values and retry.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int? id, CancellationToken token)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genre = await _genreRepository.GetGenreById(id, token);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id, CancellationToken token)
        {
            var author = await _genreRepository.GetGenreById(id, token);

            if (author == null)
            {
                return NotFound();
            }

            await _genreRepository.DeleteGenre(id, token);
            return NoContent();
        }

        [HttpPut("{id}/{name}")]
        public async Task<IActionResult> EditGenre(int id, string name, CancellationToken token)
        {

            try
            {
                await _genreRepository.UpdateGenre(id, name, token);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Can't update format check input values and retry.\n " + e.Message);
            }

            return NoContent();
        }
    }
}