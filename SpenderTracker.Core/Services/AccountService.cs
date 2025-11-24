using Microsoft.EntityFrameworkCore;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Context;
using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Services;

public class AccountService : BaseService<Account, AccountDto>, IAccountService
{
    public AccountService(ApplicationContext dbContext) : base (dbContext)
    { 
    } 
}
