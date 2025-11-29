using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Interfaces;

public interface ITransactionService : IBaseService<Transaction, TransactionDto>
{
    new Task<List<TransactionListDto>> GetAll(CancellationToken ct);
}
