using SpenderTracker.Data.Interface;

namespace SpenderTracker.Data.Model;

public class TransactionTypeDto : IDto
{
    public int Id { get; set; }
    public string TypeName { get; set; } = null!; 
}
