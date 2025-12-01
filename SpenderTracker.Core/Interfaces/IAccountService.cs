using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Interfaces;

public interface IAccountService : IBaseService<Account, AccountDto>
{
    Task<bool> IsInTransactions(int id, CancellationToken ct);
}
