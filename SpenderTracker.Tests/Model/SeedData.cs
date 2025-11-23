using SpenderTracker.Data.Model;

namespace SpenderTracker.Tests.Model;

public class SeedData
{
    public Account[] Accounts { get; set; } = null!;
    public Budget[] Budgets { get; set; } = null!;
    public TransactionGroup[] TransactionGroups { get; set; } = null!;
    public TransactionType[] TransactionTypes { get; set; } = null!;
    public TransactionMethod[] TransactionMethods { get; set; } = null!;
    public Transaction[] Transactions { get; set; } = null!;
}

