using Microsoft.AspNetCore.Mvc;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Dto;

namespace SpenderTracker.API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        AccountDto? dto = await _accountService.GetById(id, ct);
        if (dto == null)
        {
            return NotFound($"Failed to get Account with id {id}.");
        }

        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var dtos = await _accountService.GetAll(ct);
        if (dtos == null)
        {
            return StatusCode(500, "An error occurred while retrieving Accounts.");
        }

        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] AccountDto dto, CancellationToken ct)
    {
        if (dto == null)
        {
            return BadRequest("Account must be included in the body");
        }

        AccountDto? account = await _accountService.Insert(dto, ct);
        if (account == null)
        {
            return StatusCode(500, "An error occurred while creating the Account.");
        }

        return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AccountDto dto, CancellationToken ct)
    {
        if (dto == null)
        {
            return BadRequest("Account must be included in the body");
        }

        if (dto.Id != id)
        {
            return BadRequest("Account id does not match specified id.");
        } 

        bool success = await _accountService.Update(dto, ct);
        if (!success)
        {
            return StatusCode(500, "An error occurred while updating the Account.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    { 
        bool success = await _accountService.Delete(id, ct);
        if (!success)
        {
            return StatusCode(500, "An error occurred while deleting the Account.");
        }

        return NoContent();
    } 
}
