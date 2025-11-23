using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Core.Interfaces;

public interface IAccountService : IBaseService<Account, AccountDto>
{
    List<MethodListDto> GetAllMethods(); 
}
