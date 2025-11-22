using Microsoft.EntityFrameworkCore;
using SpenderTracker.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpenderTracker.Data.Model;

public class TransactionType : IEntity<TransactionTypeDto>
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string TypeName { get; set; } = null!;

    [InverseProperty(nameof(Transaction.TransactionType))]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public TransactionTypeDto ToDto()
    {
        return new TransactionTypeDto
        {
            Id = this.Id,
            TypeName = this.TypeName
        };
    }
}
