using Microsoft.AspNetCore.Mvc;
using BooksApi.Repository.Interfaces;
using BooksApi.Models;

namespace BooksApi.Controllers {
    [ApiController]
    [Route("api/author")]
    public class AuthorController : ControllerBase {
        private readonly IAuthorRepository _authorRepository;
        public AuthorController(IAuthorRepository authorRepository) {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        public async Task <ActionResult<ICollection<Author>>> GetAuthors(CancellationToken cancellationToken) {
            var authors = await _authorRepository.GetAuthors(cancellationToken);
            return Ok(authors);
        }

        [HttpPost]
        public async Task<ActionResult<Author>> AddAuthor([Bind("Name, CreatedAt, UpdatedAt")] Author author,
            CancellationToken token)
        {
            ModelState.Remove("Books");
            var newAuthor = await _authorRepository.CreateAuthor(author, token);
            return CreatedAtAction(nameof(GetAuthors), new {id = newAuthor.Id}, newAuthor);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Author>> GetAuthor(int? id, CancellationToken token)
        {
            if (id == null)
            {
                return NotFound();
            }
            var author = await _authorRepository.GetAuthorById(id, token);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Author>> DeleteAuthor(int id, CancellationToken token)
        {
            var author = await _authorRepository.GetAuthorById(id, token);

            if(author == null)
            {
                return NotFound();
            }

            await _authorRepository.DeleteAuthor(id, token);
            return NoContent();
        }
    }
}