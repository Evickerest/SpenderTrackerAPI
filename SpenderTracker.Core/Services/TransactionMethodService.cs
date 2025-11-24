using Microsoft.EntityFrameworkCore;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Context;
using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Services;

public class TransactionMethodService : BaseService<TransactionMethod, TransactionMethodDto>, ITransactionMethodService
{ 
    public TransactionMethodService(ApplicationContext dbContext) : base (dbContext)
    { 
    } 
}
