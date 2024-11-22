using nagyv_projekt.Entities;

namespace nagyv_projekt.Services;

public interface IReaderService
{
    Task AddReaderAsync(string name, string address, DateOnly birthDate);
    
    Task<List<Readers>> GetAllReadersAsync();
    
    Task<List<Readers>> GetAllReadersByNameAsync(string name);
    
    Task<Readers> GetReaderByIdAsync(Guid id); //TODO ?
    
    Task UpdateReaderAsync(Readers reader);
    
    Task DeleteReaderAsync(Guid id);
}