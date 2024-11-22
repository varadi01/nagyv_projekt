using Microsoft.AspNetCore.Mvc;
using nagyv_projekt.Entities;
using nagyv_projekt.Services;

namespace nagyv_projekt.Controllers;

[ApiController]
[Route("books")]
public class BookController : ControllerBase
{
    private IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddBook([FromBody] Books book)
    {
        await _bookService.AddBookAsync(book.Title, book.Author, book.Publisher, book.Year);
        return Ok();
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("getByTitle/{title}")]
    public async Task<IActionResult> GetBookByTitle(string title)
    {
        var books = await _bookService.GetBooksByTitleAsync(title);
        return Ok(books);
    }

    [HttpGet("getByAuthor/{author}")]
    public async Task<IActionResult> GetBooksByAuthor(string author)
    {
        var books = await _bookService.GetBooksByAuthorAsync(author);
        return Ok(books);
    }

    [HttpGet("getByPublisher/{publisher}")]
    public async Task<IActionResult> GetBooksByPublisher(string publisher)
    {
        var books = await _bookService.GetBooksByPublisherAsync(publisher);
        return Ok(books);
    }

    [HttpGet("getById/{id:guid}")]
    public async Task<IActionResult> GetBookById(Guid id)
    {
        //TODO if it doesent exist we crash
        var book = await _bookService.GetBookByIdAsync(id);
        return book == null ? NotFound() : Ok(book);
    }

    [HttpGet("getByYear/{year}")]
    public async Task<IActionResult> GetBooksByYear(int year)
    {
        var books = await _bookService.GetBooksByYearAsync(year);
        return Ok(books);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateBook([FromBody] Books book)
    {
        await _bookService.UpdateBookAsync(book);
        return Ok();
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        await _bookService.DeleteBookAsync(id);
        return Ok();
    }
}