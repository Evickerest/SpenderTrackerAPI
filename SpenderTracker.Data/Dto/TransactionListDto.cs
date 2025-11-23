using SpenderTracker.Data.Interface;

namespace SpenderTracker.Data.Dto;

public class TransactionListDto : IDto
{
    public int Id { get; set; }
    public string TransactionTypeName { get; set; } = null!;
    public string TransactionGroupName { get; set; } = null!;
    public string TransactionMethodName { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public decimal Amount { get; set; } 
    public string? Description { get; set; } 
    public DateTimeOffset Timestamp { get; set; } 
}
