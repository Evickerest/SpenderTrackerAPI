using SpenderTracker.Data.Interface;

namespace SpenderTracker.Data.Dto;

public class TransactionDto : IDto
{
    public int Id { get; set; } 
    public int TransactionTypeId { get; set; } 
    public int TransactionGroupId { get; set; } 
    public int TransactionMethodId { get; set; } 
    public int AccountId { get; set; }
    public decimal Amount { get; set; } 
    public string? Description { get; set; } 
    public DateTimeOffset Timestamp { get; set; } 
}
