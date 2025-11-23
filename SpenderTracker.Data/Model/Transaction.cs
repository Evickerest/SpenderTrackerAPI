using Microsoft.EntityFrameworkCore;
using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpenderTracker.Data.Model;

public class Transaction : IEntity<TransactionDto>
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
    public virtual TransactionType TransactionType { get; set; } = null!;

    [InverseProperty(nameof(TransactionGroup.Transactions))]
    [ForeignKey(nameof(TransactionGroupId))]
    public virtual TransactionGroup TransactionGroup { get; set; } = null!;

    [InverseProperty(nameof(TransactionMethod.Transactions))]
    [ForeignKey(nameof(TransactionMethodId))]
    public virtual TransactionMethod TransactionMethod { get; set; } = null!;

    public TransactionDto ToDto()
    {
        return new TransactionDto
        {
            Id = this.Id,
            TransactionTypeId = this.TransactionTypeId,
            TransactionGroupId = this.TransactionGroupId,
            TransactionMethodId = this.TransactionMethodId,
            Amount = this.Amount,
            Description = this.Description,
            Timestamp = this.Timestamp
        };
    }
}
