using SpenderTracker.Data.Interface;

namespace SpenderTracker.Data.Dto;

public class TransactionListDto : IDto
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public string Group { get; set; } = null!;
    public string Method { get; set; } = null!;
    public string Account { get; set; } = null!;
    public decimal Amount { get; set; } 
    public string? Description { get; set; } 
    public DateTimeOffset Timestamp { get; set; } 
}
