using Microsoft.EntityFrameworkCore;
using nagyv_projekt.Contexts;
using nagyv_projekt.Entities;

namespace nagyv_projekt.Services;

public class ReaderService : IReaderService
{
    private readonly ILogger<ReaderService> _logger;
    private readonly AppDbContext _appDbContext;

    public ReaderService(ILogger<ReaderService> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        _appDbContext = appDbContext;
    }


    public async Task AddReaderAsync(string name, string address, DateOnly birthDate)
    {
        _logger.LogInformation($"Adding new reader {name} to database");

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address) || birthDate.Year < 1900)
        {
            return;
        }
        
        var reader = new Readers {Name = name, Address = address, BirthDate = birthDate};
        
        await _appDbContext.Readers.AddAsync(reader);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<Readers>> GetAllReadersAsync()
    {
        return await _appDbContext.Readers.ToListAsync();
    }

    public async Task<List<Readers>> GetAllReadersByNameAsync(string name)
    {
        return await _appDbContext.Readers.Where(r => r.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        
    }

    public async Task<Readers> GetReaderByIdAsync(Guid id)
    {
        return await _appDbContext.Readers.FindAsync(id); //TODO EXCEPTION
    }

    //TODO maybe make this just one method?
    public async Task UpdateReaderAsync(Readers reader)
    {
        _logger.LogInformation($"Updating reader {reader.ReaderId}");
        
        var existingReader = await _appDbContext.Readers.FindAsync(reader.ReaderId);

        if (existingReader == null)
        {
            throw new InvalidOperationException(); //TODO EX
        }
        
        if (string.IsNullOrWhiteSpace(reader.Name) || string.IsNullOrWhiteSpace(reader.Address))
        {
            return;
        }
        
        existingReader.Name = reader.Name;
        existingReader.Address = reader.Address;
        await _appDbContext.SaveChangesAsync();
    }
    

    public async Task DeleteReaderAsync(Guid id)
    {
        _logger.LogInformation($"Deleting reader {id}");
        
        var reader = await _appDbContext.Readers.FindAsync(id);

        if (reader == null)
        {
            _logger.LogWarning($"Reader with {id} not found");
            throw new InvalidOperationException(); //TODO
        }
        
        _appDbContext.Readers.Remove(reader);
        await _appDbContext.SaveChangesAsync();
    }
}