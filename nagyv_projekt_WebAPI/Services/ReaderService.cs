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
        
        var reader = new Readers {Name = name, Address = address, BirthDate = birthDate};
        
        await _appDbContext.Readers.AddAsync(reader);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<Readers>> GetAllReadersAsync()
    {
        return await _appDbContext.Readers.ToListAsync();
    }

    public async Task<Readers> GetReaderByIdAsync(Guid id)
    {
        return await _appDbContext.Readers.FindAsync(id) ?? throw new InvalidOperationException(); //TODO EXCEPTION
    }

    //TODO maybe make this just one method?
    public async Task UpdateReaderAddressAsync(Guid id, string address)
    {
        _logger.LogInformation($"Updating reader {id}'s address {address}");
        
        var reader = await _appDbContext.Readers.FindAsync(id);

        if (reader == null)
        {
            throw new InvalidOperationException(); //TODO EX
        }
        
        reader.Address = address;
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateReaderNameAsync(Guid id, string name)
    {
        _logger.LogInformation($"Updating reader {name} in database");
        
        var reader = await _appDbContext.Readers.FindAsync(id);

        if (reader == null)
        {
            _logger.LogWarning($"Reader with id {id} not found");
            throw new InvalidOperationException(); //TODO EX
        }
        
        reader.Name = name;
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