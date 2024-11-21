using Microsoft.EntityFrameworkCore;
using nagyv_projekt.Contexts;
using nagyv_projekt.Entities;

namespace nagyv_projekt.Services;

public class LendingService : ILendingService
{
    private readonly ILogger<LendingService> _logger;
    private readonly AppDbContext _context;

    public LendingService(ILogger<LendingService> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    public async Task LendBookAsync(Guid bookId, Guid readerId, DateOnly returnDate)
    {
        _logger.LogInformation("Adding lending to database");

        if (IsBookLent(bookId))
        {
            _logger.LogWarning("This book is already lent");
            return; //TODO
        }
        
        var lending = new Lending {ReaderId = readerId, BookId = bookId,
            LendingDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ReturnDate = returnDate};
        
        await _context.Lending.AddAsync(lending);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Lending>> GetLendingsAsync()
    {
        return await _context.Lending.ToListAsync();
    }

    public async Task<List<Books>> GetLentBooksByReaderIdAsync(Guid readerId)
    {
        var lendings = await _context.Lending.ToListAsync();
        var booksLentToReader = lendings.Where(l => l.ReaderId == readerId)
            .Select((l) => l.BookId).ToList();
        
        var books = await _context.Books.ToListAsync();

        return books.Where((b) => booksLentToReader.Contains(b.BookID)).ToList();
    }

    public async Task UpdateLendingAsync(Lending lending)
    {
        _logger.LogInformation($"Updating lending {lending.Id} in database");
        
        var lendingToUpdate = await _context.Lending.FindAsync(lending.Id);

        if (lendingToUpdate == null)
        {
            _logger.LogWarning($"Could not find lending with id {lending.Id}");
            throw new InvalidOperationException($"Lending with id {lending.Id} was not found");
        }
        
        lendingToUpdate.ReturnDate = lending.ReturnDate;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLendingAsync(Guid lendingId)
    {
        _logger.LogInformation($"Deleting lending with id {lendingId}");
        
        var lendingToDelete = await _context.Lending.FindAsync(lendingId);

        if (lendingToDelete == null)
        {
            _logger.LogWarning($"Could not find lending with id {lendingId}");
            throw new InvalidOperationException($"Lending with id {lendingId} was not found");
        }
        
        _context.Lending.Remove(lendingToDelete);
        await _context.SaveChangesAsync();
    }

    private bool IsBookLent(Guid bookId)
    {
        return _context.Lending.Any(l => l.BookId == bookId &&
                                         l.ReturnDate > DateOnly.FromDateTime(DateTime.UtcNow));
    }
}