using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Interfaces;

public interface ITransactionService : IBaseService<Transaction, TransactionDto>
{
    new List<TransactionListDto> GetAll();
}
