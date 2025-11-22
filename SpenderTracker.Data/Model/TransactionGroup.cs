using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpenderTracker.Data.Model;

public class TransactionGroup
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string GroupName { get; set; } = null!;

    [InverseProperty(nameof(Budget.TransactionGroup))]
    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    [InverseProperty(nameof(Transaction.TransactionGroup))]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
