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

    new public async Task<List<TransactionListDto>> GetAll(CancellationToken ct)
    {
        return await _dbContext.Transactions.AsNoTracking().
            OrderByDescending(t => t.Timestamp).
            Select(t => new TransactionListDto()
            {
                Id = t.Id,
                Type = t.TransactionType.TypeName,
                Group = t.TransactionGroup.GroupName,
                Method = t.TransactionMethod.MethodName,
                Account = t.Account.AccountName,
                Description = t.Description,
                Timestamp = t.Timestamp 
            }).
            ToListAsync(ct);
    } 
}
