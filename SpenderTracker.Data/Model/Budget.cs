using Microsoft.EntityFrameworkCore;
using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpenderTracker.Data.Model;

public class Budget : IEntity<BudgetDto>
{
    [Key]
    public int Id { get; set; }

    public int? TransactionGroupId { get; set; }

    [Precision(19, 4)]
    public decimal GoalAmount { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    [InverseProperty(nameof(TransactionGroup.Budgets))]
    [ForeignKey(nameof(TransactionGroupId))]
    public virtual TransactionGroup? TransactionGroup { get; set; }

    public BudgetDto ToDto()
    {
        return new BudgetDto
        {
            Id = this.Id,
            TransactionGroupId = this.TransactionGroupId,
            GoalAmount = this.GoalAmount,
            Year = this.Year,
            Month = this.Month
        };
    } 
}
