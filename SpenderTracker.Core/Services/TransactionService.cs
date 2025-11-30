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

    public async Task<List<TransactionListDto>> GetAll(int? typeId, int? groupId, int? methodId, int? accountId, CancellationToken ct)
    {
        return await _dbContext.Transactions.AsNoTracking().
            Where(t => typeId == null || t.TransactionTypeId == typeId).
            Where(t => groupId == null || t.TransactionGroupId == groupId).
            Where(t => methodId == null || t.TransactionMethodId == methodId).
            Where(t => accountId == null || t.AccountId == accountId).
            OrderByDescending(t => t.Timestamp).
            Select(t => new TransactionListDto()
            {
                Id = t.Id,
                Type = t.TransactionType.TypeName,
                Group = t.TransactionGroup.GroupName,
                Method = t.TransactionMethod.MethodName,
                Account = t.Account.AccountName,
                Description = t.Description,
                Amount = t.Amount,
                Timestamp = t.Timestamp 
            }).
            ToListAsync(ct);
    } 
}
