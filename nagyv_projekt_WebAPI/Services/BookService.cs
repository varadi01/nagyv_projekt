using Microsoft.EntityFrameworkCore;
using nagyv_projekt.Contexts;
using nagyv_projekt.Entities;

namespace nagyv_projekt.Services;

public class BookService : IBookService
{
    private readonly ILogger<BookService> _logger;
    private readonly AppDbContext _context;

    public BookService(ILogger<BookService> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    public async Task AddBookAsync(string title, string author, string publisher, int year)
    {
        _logger.LogInformation("Adding book {title} to database", title);
        
        var book = new Books {Title = title, Author = author, Publisher = publisher, Year = year};

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task<Books> GetBookByIdAsync(Guid id)
    {
        return await _context.Books.FindAsync(id) ?? throw new InvalidOperationException(); //TODO
    }

    public async Task<List<Books>> GetBooksByAuthorAsync(string author)
    {
        var books = await _context.Books.ToListAsync();
        
        return books.Where(b => b.Author == author).ToList();
    }

    public async Task<List<Books>> GetBooksByPublisherAsync(string publisher)
    {
        var books = await _context.Books.ToListAsync();
        
        return books.Where(b => b.Publisher == publisher).ToList();
    }

    public async Task<List<Books>> GetBooksByYearAsync(int year)
    {
        var books = await _context.Books.ToListAsync();
        
        return books.Where(b => b.Year == year).ToList();
    }

    public async Task UpdateBookAsync(Books book)
    {
        _logger.LogInformation("Updating book {title} in database", book.Title);
        
        var existingBook = await GetBookByIdAsync(book.BookID);

        if (existingBook == null)
        {
            throw new InvalidOperationException(); //TODO
        }
        
        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Publisher = book.Publisher;
        existingBook.Year = book.Year;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(Guid id)
    {
        _logger.LogInformation("Deleting book {id}", id);
        
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            throw new InvalidOperationException(); //TODO
        }
        
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}