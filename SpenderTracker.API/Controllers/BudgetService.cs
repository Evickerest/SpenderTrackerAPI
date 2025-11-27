using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Dto;

namespace SpenderTracker.API.Controllers;

[Route("api/budgets")]
[ApiController]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService; 

    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        BudgetDto? dto = await _budgetService.GetById(id, ct);
        if (dto == null)
        {
            return NotFound($"Could not find Budget with specified id {id}.");
        }

        return Ok(dto); 
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var dtos = await _budgetService.GetAll(ct); 
        if (dtos == null)
        {
            return StatusCode(500, "An error occurred while retrieving Budgets.");
        }

        return Ok(dtos); 
    }

    [HttpPost]
    public IActionResult Insert([FromBody] BudgetDto dto)
    { 
        if (dto == null)
        {
            return BadRequest("Budget must be included in the body");
        }

        BudgetDto? budget = _budgetService.Insert(dto); 
        if (budget == null)
        {
            return StatusCode(500, "An error occurred while creating the Budget.");
        } 

        return CreatedAtAction(nameof(GetById), new { id = budget.Id }, budget); 
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] BudgetDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Budget must be included in the body");
        }

        if (dto.Id != id)
        {
            return BadRequest("Budget id does not match specified id.");
        } 

        if (!_budgetService.DoesExist(id))
        {
            return NotFound($"Could not find Budget with specified id {id}.");
        } 

        bool success = _budgetService.Update(dto); 
        if (!success)
        {
            return StatusCode(500, "An error occurred while updating the Budget.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        BudgetDto? dto = _budgetService.GetById(id);
        if (dto == null)
        {
            return NotFound($"Could not find Budget with specified id {id}.");
        } 

        bool success = _budgetService.Delete(dto); 
        if (!success)
        {
            return StatusCode(500, "An error occurred while deleting the Budget.");
        }

        return NoContent();
    }
}
