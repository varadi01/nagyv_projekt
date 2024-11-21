using Microsoft.AspNetCore.Mvc;
using nagyv_projekt.Entities;
using nagyv_projekt.Services;

namespace nagyv_projekt.Controllers;

[ApiController]
[Route("reader")]
public class ReaderController : ControllerBase
{
    private readonly IReaderService _readerService;

    public ReaderController(IReaderService readerService)
    {
        _readerService = readerService;
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddReader([FromBody] Readers reader)
    {
        await _readerService.AddReaderAsync(reader.Name, reader.Address, reader.BirthDate);
        return Ok();
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllReaders()
    {
        return Ok(await _readerService.GetAllReadersAsync());
    }

    [HttpGet("getById/{id}")]
    public async Task<IActionResult> GetReaderById(Guid id)
    {
        //TODO crashes if doesent exist
        var reader = await _readerService.GetReaderByIdAsync(id);
        return reader == null ? NotFound() : Ok(reader);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateReader([FromBody] Readers reader)
    {
        await _readerService.UpdateReaderAsync(reader);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteReader(Guid id)
    {
        await _readerService.DeleteReaderAsync(id);
        return Ok();
    }
}