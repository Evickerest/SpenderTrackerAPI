using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpenderTracker.Data.Model;

public class Transaction
{
    [Key]
    public int Id { get; set; }

    public int TransactionTypeId { get; set; }

    public int TransactionGroupId { get; set; }

    public int TransactionMethodId { get; set; }

    [Precision(19, 4)]
    public decimal Amount { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Description { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? ReceiptImagePath { get; set; }

    [InverseProperty(nameof(TransactionType.Transactions))]
    [ForeignKey(nameof(TransactionTypeId))]
    public virtual TransactionType? TransactionType { get; set; }

    [InverseProperty(nameof(TransactionGroup.Transactions))]
    [ForeignKey(nameof(TransactionGroupId))]
    public virtual TransactionGroup? TransactionGroup { get; set; }

    [InverseProperty(nameof(TransactionMethod.Transactions))]
    [ForeignKey(nameof(TransactionMethodId))]
    public virtual TransactionMethod? TransactionMethod { get; set; }
}
