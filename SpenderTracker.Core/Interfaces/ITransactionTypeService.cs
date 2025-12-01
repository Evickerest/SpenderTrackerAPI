using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Interfaces;

public interface ITransactionTypeService : IBaseService<TransactionType, TransactionTypeDto>
{
    Task<bool> IsInTransactions(int id, CancellationToken ct);
}
