using Microsoft.AspNetCore.Mvc;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Dto;

namespace SpenderTracker.API.Controllers;

[Route("api/transaction-types")]
[ApiController]
public class TransactionTypeController : ControllerBase
{
    private readonly ITransactionTypeService _typeService;

    public TransactionTypeController(ITransactionTypeService transactionService)
    {
        _typeService = transactionService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        TransactionTypeDto? dto = await _typeService.GetById(id, ct);
        if (dto == null)
        {
            return NotFound($"Could not find Transaction Type with specified id {id}.");
        }

        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var dtos = await _typeService.GetAll(ct);
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] TransactionTypeDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Transaction Type must be included in the body");
        }

        TransactionTypeDto? type = await _typeService.Insert(dto);
        if (type == null)
        {
            return StatusCode(500, "An error occurred while creating the TransactionType.");
        }

        return CreatedAtAction(nameof(GetById), new { id = type.Id }, type);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TransactionTypeDto dto, CancellationToken ct)
    {
        if (dto == null)
        {
            return BadRequest("Transaction Type must be included in the body");
        }

        if (dto.Id != id)
        {
            return BadRequest("Transaction Type id does not match specified id.");
        }

        if (!await _typeService.DoesExist(id, ct))
        {
            return NotFound($"Could not find Transaction Type with specified id {id}.");
        }

        bool success = await _typeService.Update(dto);
        if (!success)
        {
            return StatusCode(500, "An error occurred while updating the Transaction Type.");
        }

        return NoContent(); 
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        if (!await _typeService.DoesExist(id, ct))
        {
            return NotFound($"Could not find Transaction Type with specified id {id}.");
        }

        if (await _typeService.IsInTransactions(id, ct)) {             
            return BadRequest("Cannot delete Transaction Type as it is in at least one transaction.");
        }

        bool success = await _typeService.Delete(id);
        if (!success)
        {
            return StatusCode(500, "An error occurred while deleting the Transaction Type.");
        }

        return NoContent();
    }
}
