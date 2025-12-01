using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Interfaces;

public interface ITransactionMethodService : IBaseService<TransactionMethod, TransactionMethodDto>
{
    Task<bool> IsInTransactions(int id, CancellationToken ct);
}
