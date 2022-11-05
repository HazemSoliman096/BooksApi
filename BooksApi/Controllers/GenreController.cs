using Microsoft.AspNetCore.Mvc;
using BooksApi.Repository.Interfaces;
using BooksApi.Models;

namespace BooksApi.Controllers {
    [ApiController]
    [Route("api/genre")]
    public class GenreController : ControllerBase {
        private readonly IGenreRepository _genreRepository;
        public GenreController(IGenreRepository genreRepository) {
            _genreRepository = genreRepository;
        }

        [HttpGet]
        public async Task <ICollection<Genre>> GetGenres(CancellationToken cancellationToken) {
            var formats = await _genreRepository.GetGenres(cancellationToken);
            return formats;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult<Genre>> AddGenre([Bind("Name, CreatedAt, UpdatedAt")] Genre format,
            CancellationToken token)
        {
            ModelState.Remove("Books");
            var newGenre = await _genreRepository.CreateGenre(format, token);
            return CreatedAtAction(nameof(GetGenres), new {id = newGenre.Id}, newGenre);
        }

        [HttpGet("id")]
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

            return genre;
        }
    }
}