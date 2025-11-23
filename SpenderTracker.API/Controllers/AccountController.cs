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

    [HttpGet("methods")]
    public IActionResult GetAllMethods()
    {
        return Ok(_accountService.GetAllMethods());
    }

    [HttpGet("{accountId:int}/methods/{methodId:int}")]
    public IActionResult GetMethodById(int accountId, int methodId) {
        if (!_accountService.DoesExist(accountId))
        {
            return NotFound($"Could not find Account with specified id {accountId}.");
        }

        TransactionMethodDto? method = _methodService.GetById(methodId);
        if (method == null)
        {
            return NotFound($"Could not find Transaction Method with specified id {methodId}");
        }

        return Ok(method);
    }

    [HttpGet("{accountId:int}/methods")]
    public IActionResult GetMethods(int accountId)
    {
        if (!_accountService.DoesExist(accountId))
        {
            return NotFound($"Could not find Account with specified id {accountId}.");
        }

        return Ok(_methodService.GetAllByAccountId(accountId));
    }

    [HttpPost("{accountId:int}/methods")]
    public IActionResult InsertMethod(int accountId, [FromBody] TransactionMethodDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Transaction Method must be included in the body");
        }

        if (dto.AccountId != accountId)
        {
            return BadRequest("Account id does not match specified id.");
        }

        if (!_accountService.DoesExist(accountId))
        {
            return NotFound($"Could not find Account with specified id {accountId}.");
        }

        TransactionMethodDto? method = _methodService.Insert(dto);
        if (method == null)
        {
            return StatusCode(500, "An error occurred while creating the Transaction Method.");
        }

        return CreatedAtAction(nameof(GetMethodById), new { accountId, methodId = method.Id }, method);
    }

    [HttpPut("{accountId:int}/methods/{methodId:int}")]
    public IActionResult UpdateMethod(int accountId, int methodId, [FromBody] TransactionMethodDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Transaction Method must be included in the body");
        }

        if (dto.Id != methodId)
        {
            return BadRequest("Transaction Method id does not match specified id.");
        }

        if (dto.AccountId != accountId)
        {
            return BadRequest("Account id does not match specified id.");
        }

        if (!_accountService.DoesExist(accountId))
        {
            return NotFound($"Could not find Account with specified id {accountId}.");
        }

        if (!_methodService.DoesExist(methodId))
        {
            return NotFound($"Could not find Transaction Method with specified id {methodId}");
        }

        bool success = _methodService.Update(dto);
        if (!success)
        {
            return StatusCode(500, "An error occurred while updating the Transaction Method.");
        }

        return NoContent();
    }

    [HttpDelete("{accountId:int}/methods/{methodId:int}")]
    public IActionResult DeleteMethod(int accountId, int methodId)
    { 
        if (!_accountService.DoesExist(accountId))
        {
            return NotFound($"Could not find Account with specified id {accountId}.");
        }

        TransactionMethodDto? dto = _methodService.GetById(methodId);
        if (dto == null)
        {
            return NotFound($"Could not find Transaction Method with specified id {methodId}");
        }

        bool success = _methodService.Delete(dto);
        if (!success)
        {
            return StatusCode(500, "An error occurred while deleting the Transaction Method.");
        }

        return NoContent();
    } 
}
