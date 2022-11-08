using BooksApi.Models;
using BooksApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Book>>> GetBooks(CancellationToken cancellationToken)
        {
            ICollection<Book> books = await _bookRepository.GetBooks(cancellationToken);
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook([Bind("Title, AuthorId, GenreId, FormatId, Pages, CreatedAt, UpdatedAt")] Book book,
            CancellationToken token)
        {
            ModelState.Remove("Author");
            ModelState.Remove("Genre");
            ModelState.Remove("Format");

            Book? newBook = await _bookRepository.CreateBook(book, token);

            if (newBook != null)
            {
                return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, newBook);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Can't create author check input values and retry.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int? id, CancellationToken token)
        {
            if (id == null)
            {
                return NotFound();
            }
            Book? book = await _bookRepository.GetBookById(id, token);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id, CancellationToken token)
        {
            Book? book = await _bookRepository.GetBookById(id, token);

            if (book == null)
            {
                return NotFound();
            }

            await _bookRepository.DeleteBook(id, token);
            return NoContent();
        }

        [HttpPut("{id}/{name}")]
        public async Task<IActionResult> EditBook(int id, [Bind("Title, AuthorId, GenreId, FormatId, Pages, CreatedAt, UpdatedAt")] Book book, CancellationToken token)
        {
            if(id != book.Id || book == null)
            {
                return BadRequest();
            }

            try
            {
                await _bookRepository.UpdateBook(id, book, token);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Can't update author check input values and retry.\n " + e.Message);
            }

            return NoContent();
        }
    }
}
