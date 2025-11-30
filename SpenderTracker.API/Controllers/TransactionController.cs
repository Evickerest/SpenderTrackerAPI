using Microsoft.AspNetCore.Mvc;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Dto;

namespace SpenderTracker.API.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        TransactionDto? dto = await _transactionService.GetById(id, ct);
        if (dto == null)
        {
            return NotFound($"Could not find Transaction with specified id {id}.");
        }

        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct, 
        [FromQuery] int? typeId, [FromQuery] int? groupId, [FromQuery] int? methodId, [FromQuery] int? accountId)
    {
        var dtos = await _transactionService.GetAll(typeId, groupId, methodId, accountId, ct);
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] TransactionDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Transaction must be included in the body");
        }

        TransactionDto? transaction = await _transactionService.Insert(dto);
        if (transaction == null)
        {
            return StatusCode(500, "An error occurred while creating the Transaction.");
        }

        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TransactionDto dto, CancellationToken ct)
    {
        if (dto == null)
        {
            return BadRequest("Transaction must be included in the body");
        }

        if (dto.Id != id)
        {
            return BadRequest("Transaction id does not match specified id.");
        }

        if (!await _transactionService.DoesExist(id, ct))
        {
            return NotFound($"Could not find Transaction with specified id {id}.");
        } 

        bool success = await _transactionService.Update(dto);
        if (!success)
        {
            return StatusCode(500, "An error occurred while updating the Transaction.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        if (!await _transactionService.DoesExist(id, ct))
        {
            return NotFound($"Could not find Transaction with specified id {id}."); 
        } 

        bool success = await  _transactionService.Delete(id);
        if (!success)
        {
            return StatusCode(500, "An error occurred while deleting the Transaction.");
        }

        return NoContent();
    }
}
