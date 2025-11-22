using SpenderTracker.Data.Interface;

namespace SpenderTracker.Data.Dto;

public class BudgetDto : IDto
{
    public int Id { get; set; } 
    public int? TransactionGroupId { get; set; } 
    public decimal GoalAmount { get; set; } 
    public int Year { get; set; } 
    public int Month { get; set; } 
}
