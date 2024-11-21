using nagyv_projekt.Entities;

namespace nagyv_projekt.Services;

public interface IReaderService
{
    Task AddReaderAsync(string name, string address, DateOnly birthDate);
    
    Task<List<Readers>> GetAllReadersAsync();
    
    Task<Readers> GetReaderByIdAsync(Guid id); //TODO ?
    
    Task UpdateReaderAddressAsync(Guid id, string address);
    
    Task UpdateReaderNameAsync(Guid id, string name);
    
    Task DeleteReaderAsync(Guid id);
}