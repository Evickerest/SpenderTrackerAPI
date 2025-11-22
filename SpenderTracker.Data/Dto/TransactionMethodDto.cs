using SpenderTracker.Data.Interface;

namespace SpenderTracker.Data.Model;

public class TransactionMethodDto : IDto
{
    public int Id { get; set; }
    public string MethodName { get; set; } = null!; 
    public int AccountId { get; set; }
}
