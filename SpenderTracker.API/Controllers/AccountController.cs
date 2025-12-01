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
            return NotFound($"Could not find Account with specified id {id}");
        }

        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var dtos = await _accountService.GetAll(ct); 
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] AccountDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Account must be included in the body");
        } 

        AccountDto? account = await _accountService.Insert(dto);
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

        if (!await _accountService.DoesExist(id, ct))
        {
           return NotFound($"Account with id {id} does not exist.");
        } 

        bool success = await _accountService.Update(dto);
        if (!success)
        {
            return StatusCode(500, "An error occurred while updating the Account.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    { 
        if (!await _accountService.DoesExist(id, ct))
        {
           return NotFound($"Account with id {id} does not exist.");
        } 

        if (await _accountService.IsInTransactions(id, ct)) {             
            return BadRequest("Cannot delete Account as it is in at least one transaction.");
        }

        bool success = await _accountService.Delete(id);
        if (!success)
        {
            return StatusCode(500, "An error occurred while deleting the Account.");
        }

        return NoContent();
    } 
}
