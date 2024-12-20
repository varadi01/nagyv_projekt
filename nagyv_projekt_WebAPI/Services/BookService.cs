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

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(publisher) || year < 0)
        {
            return;
        }
        
        var book = new Books {Title = title, Author = author, Publisher = publisher, Year = year};

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task<Books> GetBookByIdAsync(Guid id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<List<Books>> GetAllBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<List<Books>> GetBooksByTitleAsync(string title)
    {
        return await _context.Books.Where(b => b.Title.Contains(title)).ToListAsync();
    }

    public async Task<List<Books>> GetBooksByAuthorAsync(string author)
    {
        var books = await _context.Books.ToListAsync();
        
        return books.Where(b => b.Author.Contains(author)).ToList();
    }

    public async Task<List<Books>> GetBooksByPublisherAsync(string publisher)
    {
        var books = await _context.Books.ToListAsync();
        
        return books.Where(b => b.Publisher.Contains(publisher)).ToList();
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
            throw new InvalidOperationException(); 
        }
        
        if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author) ||
            string.IsNullOrWhiteSpace(book.Publisher) || book.Year < 0)
        {
            return;
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
            throw new InvalidOperationException(); 
        }
        
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}