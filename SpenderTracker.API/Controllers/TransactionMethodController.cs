using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Dto;

namespace SpenderTracker.API.Controllers;

[Route("api/transaction-methods")]
[ApiController]
public class TransactionMethodController : ControllerBase 
{
    public readonly ITransactionMethodService _methodService; 

    public TransactionMethodController(ITransactionMethodService methodService)
    {
        _methodService = methodService; 
    } 

    [HttpGet("")]
    public IActionResult GetAll()
    {
        return Ok(_methodService.GetAll());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    { 
        TransactionMethodDto? method = _methodService.GetById(id);
        if (method == null)
        {
            return NotFound($"Could not find Transaction Method with specified id {id}");
        }

        return Ok(method);
    }

    [HttpPost("")]
    public IActionResult InsertMethod([FromBody] TransactionMethodDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Transaction Method must be included in the body");
        } 

        TransactionMethodDto? method = _methodService.Insert(dto);
        if (method == null)
        {
            return StatusCode(500, "An error occurred while creating the Transaction Method.");
        }

        return CreatedAtAction(nameof(GetById), new { id = method.Id }, method);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateMethod(int id, [FromBody] TransactionMethodDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Transaction Method must be included in the body");
        }

        if (dto.Id != id)
        {
            return BadRequest("Transaction Method id does not match specified id.");
        } 

        if (!_methodService.DoesExist(id))
        {
            return NotFound($"Could not find Transaction Method with specified id {id}");
        }

        bool success = _methodService.Update(dto);
        if (!success)
        {
            return StatusCode(500, "An error occurred while updating the Transaction Method.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteMethod(int id)
    { 
        TransactionMethodDto? dto = _methodService.GetById(id);
        if (dto == null)
        {
            return NotFound($"Could not find Transaction Method with specified id {id}");
        }

        bool success = _methodService.Delete(dto);
        if (!success)
        {
            return StatusCode(500, "An error occurred while deleting the Transaction Method.");
        }

        return NoContent();
    }
}
