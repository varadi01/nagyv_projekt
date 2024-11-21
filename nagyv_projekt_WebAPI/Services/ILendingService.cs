using nagyv_projekt.Entities;

namespace nagyv_projekt.Services;

public interface ILendingService
{
    Task LendBookAsync(Guid bookId, Guid readerId, DateOnly returnDate);
    
    Task<List<Lending>> GetLendingsAsync();
    
    Task<List<Books>> GetLentBooksByReaderIdAsync(Guid readerId);
    
    Task UpdateLendingAsync(Lending lending); //TODO ?
    
    Task DeleteLendingAsync(Guid lendingId);
}