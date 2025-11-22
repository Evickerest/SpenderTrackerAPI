using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpenderTracker.Data.Model;

public class TransactionType
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string TypeName { get; set; } = null!;

    [InverseProperty(nameof(Transaction.TransactionType))]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
