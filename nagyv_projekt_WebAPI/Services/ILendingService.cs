using nagyv_projekt.Entities;

namespace nagyv_projekt.Services;

public interface ILendingService
{
    Task LendBookAsync(Guid bookId, Guid readerId, int leasePeriod);
    
    Task<List<Lending>> GetLendingsAsync();
    
    //Task<List<Lending>> GetLendingsByReaderIdAsync(Guid userId); //TODO ?
    
    Task<List<Books>> GetLentBooksByReaderIdAsync(Guid readerId);
    
    Task UpdateLendingAsync(Lending lending); //TODO ?
    
    Task DeleteLendingAsync(Guid lendingId);
}