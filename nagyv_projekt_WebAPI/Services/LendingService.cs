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


    public async Task LendBookAsync(Guid bookId, Guid readerId, DateOnly lendingDate, DateOnly returnDate)
    {
        _logger.LogInformation("Adding lending to database");
        _logger.LogInformation($"Book Id: {bookId}, Reader Id: {readerId}");

        // if (await IsBookLent(bookId, lendingDate))
        // {
        //     _logger.LogWarning("This book is already lent");
        //     return; //TODO
        // }

        if (bookId == Guid.Empty || readerId == Guid.Empty)
        {
            return;
        }

        if (lendingDate > returnDate)
        {
            return;
        }
        
        var lending = new Lending {ReaderId = readerId, BookId = bookId,
            LendingDate = lendingDate,  
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
        
        var booksLentToReader = lendings.Where(l => l.ReaderId.Equals(readerId) &&
                                                    l.ReturnDate > DateOnly.FromDateTime(DateTime.Today))
                                                    .Select((l) => l.BookId).ToList();
        
        var books = await _context.Books.ToListAsync();
        
        return books.Where((b) => booksLentToReader.Contains(b.BookID)).ToList();
    }

    public async Task<List<Lending>> GetLendingsByReaderIdAsync(Guid readerId)
    {
        return await _context.Lending.Where(l => l.ReaderId == readerId &&
                                                 l.ReturnDate > DateOnly.FromDateTime(DateTime.Today)).ToListAsync();
    }

    public async Task UpdateLendingAsync(Lending lending)
    {
        _logger.LogInformation($"Updating lending {lending.Id} in database, new return date: {lending.ReturnDate}");
        
        var lendingToUpdate = await _context.Lending.FindAsync(lending.Id);

        if (lendingToUpdate == null)
        {
            _logger.LogWarning($"Could not find lending with id {lending.Id}");
            throw new InvalidOperationException($"Lending with id {lending.Id} was not found");
        }

        if (lending.ReturnDate < lending.LendingDate)
        {
            return;
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

    private async Task<bool> IsBookLent(Guid bookId, DateOnly lendingDate)
    {
        //TODO gets existing lendings whatever i do
        _logger.LogInformation($"Checking if {bookId} lending is lent on {lendingDate}");
        var v = await _context.Lending.Where((l) => l.BookId.Equals(bookId) && l.ReturnDate.CompareTo(lendingDate) > 0)
            .ToListAsync();
        _logger.LogInformation($"Found {v.Count} lendings : {v[0]}");
        return v.Count > 0;
    }
}