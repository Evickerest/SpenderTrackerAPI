using SpenderTracker.Data.Interface;

namespace SpenderTracker.Data.Dto;

public class AccountDto : IDto
{
    public int Id { get; set; } 
    public string AccountName { get; set; } = null!; 
    public decimal Balance { get; set; } 
}
