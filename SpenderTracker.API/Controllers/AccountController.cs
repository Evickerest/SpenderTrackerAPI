using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Dto;

namespace SpenderTracker.API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ITransactionMethodService _methodService;

    public AccountController(IAccountService accountService, ITransactionMethodService methodService)
    {
        _accountService = accountService;
        _methodService = methodService;
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        AccountDto? dto = _accountService.GetById(id);
        if (dto == null)
        {
            return NotFound($"Could not find Account with specified id {id}.");
        }

        return Ok(dto);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_accountService.GetAll());
    }

    [HttpPost]
    public IActionResult Insert([FromBody] AccountDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Account must be included in the body");
        }

        AccountDto? account = _accountService.Insert(dto);
        if (account == null)
        {
            return StatusCode(500, "An error occurred while creating the Account.");
        }

        return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] AccountDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Account must be included in the body");
        }

        if (dto.Id != id)
        {
            return BadRequest("Account id does not match specified id.");
        }

        if (!_accountService.DoesExist(id))
        {
            return NotFound($"Could not find Account with specified id {id}.");
        }

        bool success = _accountService.Update(dto);
        if (!success)
        {
            return StatusCode(500, "An error occurred while updating the Account.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        AccountDto? dto = _accountService.GetById(id);
        if (dto == null)
        {
            return NotFound($"Could not find Account with specified id {id}.");
        }

        bool success = _accountService.Delete(dto);
        if (!success)
        {
            return StatusCode(500, "An error occurred while deleting the Account.");
        }

        return NoContent();
    } 
}
