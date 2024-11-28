using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nagyv_projekt.Contexts;
using nagyv_projekt.Entities;
using nagyv_projekt.Services;

namespace nagyv_projekt_API.Tests;

public class ReaderService_Tests
{
    private static DbContextOptions<AppDbContext> _options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "BookServiceTestDb").Options;

    private AppDbContext _context;

    private ReaderService _service;

    [SetUp]
    public void Setup()
    {
        _context = new AppDbContext(_options);
        _context.Database.EnsureCreated();

        _service = new ReaderService(new Logger<ReaderService>(new LoggerFactory()), _context);
    }

    [TearDown]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddReader_ValidInputs_ShouldAddReader()
    {
        var name = "Test Reader";
        var address = "Test Address";
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20));

        await _service.AddReaderAsync(name, address, birthDate);

        var readers = await _context.Readers.ToListAsync();

        Assert.That(readers.Count, Is.EqualTo(1));
        Assert.That(readers[0].Name, Is.EqualTo(name));
    }

    [Test]
    public async Task AddReader_InValidInputs_ShouldNotAddReader()
    {
        var name = "Test Reader";
        var address = "Test Address";
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-200));

        await _service.AddReaderAsync(name, address, birthDate);

        var readers = await _context.Readers.ToListAsync();

        Assert.That(readers.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetReadersByName_ExistingName_ShouldReturnReadersWithName()
    {
        await _service.AddReaderAsync("Carley Mathers", "Test Address",
            DateOnly.FromDateTime(DateTime.Now).AddYears(-20));
        await _service.AddReaderAsync("John Mathers", "Test Address",
            DateOnly.FromDateTime(DateTime.Now).AddYears(-20));
        await _service.AddReaderAsync("Carley Smith", "Test Address",
            DateOnly.FromDateTime(DateTime.Now).AddYears(-20));
        await _service.AddReaderAsync("Rahim Sudra", "Test Address", DateOnly.FromDateTime(DateTime.Now).AddYears(-20));

        var readersWithNameCarley = await _service.GetAllReadersByNameAsync("Carley");

        Assert.That(readersWithNameCarley.Count, Is.EqualTo(2));

        var readersWithNameSmith = await _service.GetAllReadersByNameAsync("Smith");

        Assert.That(readersWithNameSmith.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetReadersByName_NonExistingName_ShouldReturnNoReaders()
    {
        await _service.AddReaderAsync("Carley Mathers", "Test Address",
            DateOnly.FromDateTime(DateTime.Now).AddYears(-20));
        await _service.AddReaderAsync("John Mathers", "Test Address",
            DateOnly.FromDateTime(DateTime.Now).AddYears(-20));
        await _service.AddReaderAsync("Carley Smith", "Test Address",
            DateOnly.FromDateTime(DateTime.Now).AddYears(-20));
        await _service.AddReaderAsync("Rahim Sudra", "Test Address", DateOnly.FromDateTime(DateTime.Now).AddYears(-20));

        var readersWithNameCarley = await _service.GetAllReadersByNameAsync("Max");

        Assert.That(readersWithNameCarley.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task UpdateReader_ValidInputs_ShouldUpdateReader()
    {
        var name = "Test Reader";
        var address = "Test Address";
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20));

        await _service.AddReaderAsync(name, address, birthDate);

        var readers = await _service.GetAllReadersAsync();

        var exsistingReader = readers.First();

        exsistingReader.Name = "new name";
        exsistingReader.Address = "new address";

        await _service.UpdateReaderAsync(exsistingReader);

        readers = await _context.Readers.ToListAsync();

        Assert.That(readers[0].Name, Is.EqualTo("new name"));
        Assert.That(readers[0].Address, Is.EqualTo("new address"));
    }

    [Test]
    public async Task UpdateReader_InValidInputs_ShouldNotUpdateReader()
    {
        var name = "Test Reader";
        var address = "Test Address";
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20));

        await _service.AddReaderAsync(name, address, birthDate);

        var readers = await _context.Readers.ToListAsync();

        var exsistingReader = readers.First();

        var modifiedReader = new Readers()
        {
            ReaderId = exsistingReader.ReaderId,
            Name = "     \n  ",
            Address = exsistingReader.Address,
            BirthDate = exsistingReader.BirthDate,
        };

        await _service.UpdateReaderAsync(modifiedReader);

        readers = await _context.Readers.ToListAsync();

        Assert.That(readers[0].Name, Is.EqualTo(name));
    }

    [Test]
    public async Task DeleteReader_ValidInputs_ShouldDeleteReader()
    {
        var name = "Test Reader";
        var address = "Test Address";
        var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20));

        await _service.AddReaderAsync(name, address, birthDate);

        var readers = await _service.GetAllReadersAsync();
        var exsistingReader = readers.First();

        await _service.DeleteReaderAsync(exsistingReader.ReaderId);

        readers = await _context.Readers.ToListAsync();
        Assert.That(readers.Count, Is.EqualTo(0));
    }
}