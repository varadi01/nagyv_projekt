using Microsoft.AspNetCore.Mvc;
using nagyv_projekt.Entities;
using nagyv_projekt.Services;

namespace nagyv_projekt.Controllers;

[ApiController]
[Route("lending")]
public class LendingController : ControllerBase
{
    private readonly ILendingService _lendingService;

    public LendingController(ILendingService lendingService)
    {
        _lendingService = lendingService;
    }

    [HttpPost]
    [Route("lend")]
    public async Task<IActionResult> RegisterLending([FromBody] Lending lending)
    {
        await _lendingService.LendBookAsync(lending.BookId, lending.ReaderId, lending.ReturnDate);
        return Ok();
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllLendings()
    {
        return Ok(await _lendingService.GetLendingsAsync());
    }

    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> GetLendingsByReaderId(Guid id)
    {
        //does not matter whether the reader exists!
        return Ok(await _lendingService.GetLentBooksByReaderIdAsync(id));
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateLending([FromBody] Lending lending)
    {
        await _lendingService.UpdateLendingAsync(lending);
        return Ok();
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteLending(Guid id)
    {
        await _lendingService.DeleteLendingAsync(id);
        return Ok();
    }
}