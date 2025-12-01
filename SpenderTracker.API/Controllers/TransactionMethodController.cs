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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    { 
        TransactionMethodDto? method = await _methodService.GetById(id, ct);
        if (method == null)
        {
            return NotFound($"Could not find Transaction Method with specified id {id}");
        }

        return Ok(method);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var dtos = await _methodService.GetAll(ct);
        return Ok(dtos);
    }

    [HttpPost("")]
    public async Task<IActionResult> InsertMethod([FromBody] TransactionMethodDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Transaction Method must be included in the body");
        } 

        TransactionMethodDto? method = await _methodService.Insert(dto);
        if (method == null)
        {
            return StatusCode(500, "An error occurred while creating the Transaction Method.");
        }

        return CreatedAtAction(nameof(GetById), new { id = method.Id }, method);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMethod(int id, [FromBody] TransactionMethodDto dto, CancellationToken ct)
    {
        if (dto == null)
        {
            return BadRequest("Transaction Method must be included in the body");
        }

        if (dto.Id != id)
        {
            return BadRequest("Transaction Method id does not match specified id.");
        } 

        if (!await _methodService.DoesExist(id, ct))
        {
            return NotFound($"Could not find Transaction Method with specified id {id}.");
        }

        bool success = await _methodService.Update(dto);
        if (!success)
        {
            return StatusCode(500, "An error occurred while updating the Transaction Method.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMethod(int id, CancellationToken ct)
    { 
        if (!await _methodService.DoesExist(id, ct))
        {
            return NotFound($"Could not find Transaction Method with specified id {id}.");
        } 

        if (await _methodService.IsInTransactions(id, ct)) {             
            return BadRequest("Cannot delete Transaction Method as it is in at least one transaction.");
        }

        bool success = await _methodService.Delete(id);
        if (!success)
        {
            return StatusCode(500, "An error occurred while deleting the Transaction Method.");
        }

        return NoContent();
    }
}
