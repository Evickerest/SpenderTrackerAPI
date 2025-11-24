using SpenderTracker.Data.Interface;

namespace SpenderTracker.Data.Dto;

public class TransactionMethodDto : IDto
{
    public int Id { get; set; }
    public string MethodName { get; set; } = null!; 
}
