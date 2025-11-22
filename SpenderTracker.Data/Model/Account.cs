using Microsoft.EntityFrameworkCore;
using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpenderTracker.Data.Model;

public class Account : IEntity<AccountDto>
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string AccountName { get; set; } = null!;

    [Precision(19, 4)]
    public decimal Balance { get; set; }

    [InverseProperty(nameof(TransactionMethod.Account))]
    public virtual ICollection<TransactionMethod> TransactionMethods { get; set; } = new List<TransactionMethod>();

    public AccountDto ToDto() {         
        return new AccountDto
        {
            Id = this.Id,
            AccountName = this.AccountName,
            Balance = this.Balance
        }; 
    }
}
