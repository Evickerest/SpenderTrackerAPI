using Microsoft.EntityFrameworkCore;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Context;
using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Services;

public class TransactionService : BaseService<Transaction, TransactionDto>, ITransactionService
{
    public TransactionService(ApplicationContext dbContext) : base (dbContext)
    { 
    } 

    new public List<TransactionDto> GetAll()
    {
        return _dbContext.Transactions.AsNoTracking().
            OrderByDescending(t => t.Timestamp).
            Select(t => t.ToDto()).
            ToList();
    }
}
