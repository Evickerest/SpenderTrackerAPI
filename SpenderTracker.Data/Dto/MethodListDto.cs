 namespace SpenderTracker.Data.Dto;

public class MethodListDto 
{
    public string AccountName { get; set; } = null!;
    public List<TransactionMethodDto> Methods { get; set; } = null!; 
}
