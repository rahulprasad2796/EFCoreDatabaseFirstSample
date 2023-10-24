using EFCoreDatabaseFirstSample.Models;
using EFCoreDatabaseFirstSample.Models.Repository;
using Microsoft.AspNetCore.Mvc;

[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IDataRepository<Book> _dataRepository;

    public BooksController(IDataRepository<Book> dataRepository)
    {
        _dataRepository = dataRepository;
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var book = _dataRepository.Get(id);
        if (book == null)
        {
            return NotFound("Book not found.");
        }

        return Ok(book);
    }
}