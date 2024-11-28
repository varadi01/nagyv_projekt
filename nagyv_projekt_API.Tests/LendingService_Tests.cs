using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nagyv_projekt.Contexts;
using nagyv_projekt.Entities;
using nagyv_projekt.Services;

namespace nagyv_projekt_API.Tests;

public class LendingService_Tests
{
    private static DbContextOptions<AppDbContext> _options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "BookServiceTestDb").Options;

    private AppDbContext _context;

    private LendingService _service;
    private BookService _bookService;
    private ReaderService _readerService;

    [SetUp]
    public void Setup()
    {
        _context = new AppDbContext(_options);
        _context.Database.EnsureCreated();

        _service = new LendingService(new Logger<LendingService>(new LoggerFactory()), _context);
        _bookService = new BookService(new Logger<BookService>(new LoggerFactory()), _context);
        _readerService = new ReaderService(new Logger<ReaderService>(new LoggerFactory()), _context);
    }

    [TearDown]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task LendBook_ValidInputs_ShouldCreateLending()
    {
        await _bookService.AddBookAsync("title", "author", "publisher", 2000);
        await _readerService.AddReaderAsync("name", "address", DateOnly.FromDateTime(DateTime.Now.AddYears(-20)));

        var existingBook = await _context.Books.FirstAsync();
        var existingReader = await _context.Readers.FirstAsync();

        await _service.LendBookAsync(existingBook.BookID, existingReader.ReaderId,
            DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7)));

        var lendings = await _service.GetLendingsAsync();
        var createdLending = lendings.First();

        Assert.That(createdLending, Is.Not.Null);
        Assert.That(createdLending.BookId, Is.EqualTo(existingBook.BookID));
        Assert.That(createdLending.ReaderId, Is.EqualTo(existingReader.ReaderId));
    }

    [Test]
    public async Task LendBook_InValidInputs_ShouldNotCreateLending()
    {
        await _bookService.AddBookAsync("title", "author", "publisher", 2000);
        await _readerService.AddReaderAsync("name", "address", DateOnly.FromDateTime(DateTime.Now.AddYears(-20)));

        var existingBook = await _context.Books.FirstAsync();
        var existingReader = await _context.Readers.FirstAsync();

        await _service.LendBookAsync(existingBook.BookID, existingReader.ReaderId,
            DateOnly.FromDateTime(DateTime.Now.AddDays(7)), DateOnly.FromDateTime(DateTime.Now));

        var lendings = await _service.GetLendingsAsync();
        Assert.That(lendings.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task GetLentBooksByReader_ValidInputs_ShouldReturnLentBooks()
    {
        await _bookService.AddBookAsync("title", "author", "publisher", 2000);
        await _bookService.AddBookAsync("title2", "author2", "publisher2", 2000);
        await _readerService.AddReaderAsync("name", "address", DateOnly.FromDateTime(DateTime.Now.AddYears(-20)));

        var existingBook = await _context.Books.FirstAsync();
        var existingBook2 = _context.Books.ToList()[1]; //meh
        var existingReader = await _context.Readers.FirstAsync();

        await _service.LendBookAsync(existingBook.BookID, existingReader.ReaderId,
            DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7)));

        await _service.LendBookAsync(existingBook2.BookID, existingReader.ReaderId,
            DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7)));

        var lendings = await _service.GetLentBooksByReaderIdAsync(existingReader.ReaderId);
        Assert.That(lendings.Count(), Is.EqualTo(2));
        Assert.That(lendings[0], Is.InstanceOf<Books>());
    }

    [Test]
    public async Task GetLendingsByReader_ValidInputs_ShouldReturnLendings()
    {
        await _bookService.AddBookAsync("title", "author", "publisher", 2000);
        await _bookService.AddBookAsync("title2", "author2", "publisher2", 2000);
        await _readerService.AddReaderAsync("name", "address", DateOnly.FromDateTime(DateTime.Now.AddYears(-20)));

        var existingBook = await _context.Books.FirstAsync();
        var existingBook2 = _context.Books.ToList()[1]; //meh
        var existingReader = await _context.Readers.FirstAsync();

        await _service.LendBookAsync(existingBook.BookID, existingReader.ReaderId,
            DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7)));

        await _service.LendBookAsync(existingBook2.BookID, existingReader.ReaderId,
            DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7)));

        var lendings = await _service.GetLendingsByReaderIdAsync(existingReader.ReaderId);
        Assert.That(lendings.Count(), Is.EqualTo(2));
        Assert.That(lendings[0], Is.InstanceOf<Lending>());
    }

    [Test]
    public async Task UpdateLending_ValidInputs_ShouldUpdateLendings()
    {
        await _bookService.AddBookAsync("title", "author", "publisher", 2000);
        await _readerService.AddReaderAsync("name", "address", DateOnly.FromDateTime(DateTime.Now.AddYears(-20)));

        var existingBook = await _context.Books.FirstAsync();
        var existingReader = await _context.Readers.FirstAsync();

        await _service.LendBookAsync(existingBook.BookID, existingReader.ReaderId,
            DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7)));

        var lendings = await _service.GetLendingsAsync();
        var createdLending = lendings.First();

        var originalReturnDate = createdLending.ReturnDate;

        createdLending.ReturnDate = createdLending.ReturnDate.AddDays(7);
        await _service.UpdateLendingAsync(createdLending);

        lendings = await _service.GetLendingsAsync();
        var updatedLending = lendings.First();

        Assert.That(updatedLending, Is.Not.Null);
        Assert.That(updatedLending.BookId, Is.EqualTo(existingBook.BookID));
        Assert.That(updatedLending.ReaderId, Is.EqualTo(existingReader.ReaderId));
        Assert.That(updatedLending.ReturnDate, Is.Not.EqualTo(originalReturnDate));
    }

    [Test]
    public async Task UpdateLending_InValidInputs_ShouldNotUpdateLendings()
    {
        await _bookService.AddBookAsync("title", "author", "publisher", 2000);
        await _readerService.AddReaderAsync("name", "address", DateOnly.FromDateTime(DateTime.Now.AddYears(-20)));

        var existingBook = await _context.Books.FirstAsync();
        var existingReader = await _context.Readers.FirstAsync();

        await _service.LendBookAsync(existingBook.BookID, existingReader.ReaderId,
            DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7)));

        var lendings = await _service.GetLendingsAsync();
        var createdLending = lendings.First();

        var originalReturnDate = createdLending.ReturnDate;

        var modifiedLending = new Lending()
        {
            BookId = createdLending.BookId, ReaderId = createdLending.ReaderId,
            LendingDate = createdLending.LendingDate, ReturnDate = createdLending.ReturnDate.AddDays(-20)
        };
        modifiedLending.Id = createdLending.Id;

        await _service.UpdateLendingAsync(modifiedLending);

        lendings = await _service.GetLendingsAsync();
        var updatedLending = lendings.First();

        Assert.That(updatedLending, Is.Not.Null);
        Assert.That(updatedLending.BookId, Is.EqualTo(existingBook.BookID));
        Assert.That(updatedLending.ReturnDate, Is.EqualTo(originalReturnDate));
    }

    [Test]
    public async Task DeleteLending_ValidInputs_ShouldDeleteLendings()
    {
        await _bookService.AddBookAsync("title", "author", "publisher", 2000);
        await _readerService.AddReaderAsync("name", "address", DateOnly.FromDateTime(DateTime.Now.AddYears(-20)));

        var existingBook = await _context.Books.FirstAsync();
        var existingReader = await _context.Readers.FirstAsync();

        await _service.LendBookAsync(existingBook.BookID, existingReader.ReaderId,
            DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7)));

        var lendings = await _service.GetLendingsAsync();
        var createdLending = lendings.First();

        await _service.DeleteLendingAsync(createdLending.Id);

        lendings = await _service.GetLendingsAsync();
        Assert.That(lendings.Count(), Is.EqualTo(0));
    }
}