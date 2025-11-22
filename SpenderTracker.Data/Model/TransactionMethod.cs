using Microsoft.EntityFrameworkCore;
using SpenderTracker.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpenderTracker.Data.Model;

public class TransactionMethod : IEntity<TransactionMethodDto>
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string MethodName { get; set; } = null!;

    public int AccountId { get; set; }

    [InverseProperty(nameof(Model.Account.TransactionMethods))]
    [ForeignKey(nameof(AccountId))]
    public virtual Account? Account { get; set; } 

    [InverseProperty(nameof(Transaction.TransactionMethod))]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public TransactionMethodDto ToDto()
    {
        return new TransactionMethodDto
        {
            Id = this.Id,
            MethodName = this.MethodName,
            AccountId = this.AccountId
        };
    }
}
