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

    new public List<TransactionListDto> GetAll()
    {
        return _dbContext.Transactions.AsNoTracking().
            Select(t => new TransactionListDto
            {
                Id = t.Id,
                TransactionTypeName = t.TransactionType.TypeName,
                TransactionGroupName = t.TransactionGroup.GroupName,
                TransactionMethodName = t.TransactionMethod.MethodName,
                AccountName = t.TransactionMethod.Account.AccountName,
                Description = t.Description,
                Amount = t.Amount,
                Timestamp = t.Timestamp
            }).
            ToList(); 
    }
}
