using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Interfaces;

public interface ITransactionService : IBaseService<Transaction, TransactionDto>
{
    Task<List<TransactionListDto>> GetAll(int? typeId, int? groupId, int? methodId, int? accountId, CancellationToken ct);
}
