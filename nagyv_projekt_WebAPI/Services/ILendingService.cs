using nagyv_projekt.Entities;

namespace nagyv_projekt.Services;

public interface ILendingService
{
    Task LendBookAsync(Guid bookId, Guid readerId,DateOnly lendingDate, DateOnly returnDate);
    
    Task<List<Lending>> GetLendingsAsync();

    Task<List<Lending>> GetLendingsByReaderIdAsync(Guid readerId);
    
    Task<List<Books>> GetLentBooksByReaderIdAsync(Guid readerId);
    
    Task UpdateLendingAsync(Lending lending);
    
    Task DeleteLendingAsync(Guid lendingId);
}