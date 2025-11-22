using Microsoft.EntityFrameworkCore;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Context;
using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Services;

public class AccountService : IAccountService
{
    private readonly ApplicationContext _dbContext;

    public AccountService(ApplicationContext dbContext)
    {
        _dbContext = dbContext; 
    }

    public AccountDto? GetById(int id)
    {
        var account = _dbContext.Accounts.Find(id);
        return account?.ToDto();
    }

    public List<AccountDto> GetAll()
    {
        return _dbContext.Accounts.AsNoTracking().
            Select(a => a.ToDto()).
            ToList();
    }

    public AccountDto? Insert(AccountDto accountDto)
    {
        Account account = new();
        _dbContext.Accounts.Entry(account).CurrentValues.SetValues(accountDto);
        _dbContext.Accounts.Add(account);

        if (_dbContext.SaveChanges() > 0)
        {
            return account.ToDto();
        } else
        {
            return null;
        } 
    }

    public bool Update(AccountDto accountDto)
    {
        Account? account = _dbContext.Accounts.Find(accountDto.Id);
        if (account == null) return false;

        _dbContext.Accounts.Entry(account).CurrentValues.SetValues(accountDto);
        return _dbContext.SaveChanges() > 0; 
    }

    public bool Delete(AccountDto accountDto)
    {
        Account? account = _dbContext.Accounts.Find(accountDto.Id);
        if (account == null) return false;

        _dbContext.Accounts.Remove(account);
        return _dbContext.SaveChanges() > 0;
    } 
}
