using Microsoft.EntityFrameworkCore;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Data.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    } 

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionGroup> TransactionGroups { get; set; }
    public DbSet<TransactionMethod> TransactionMethods { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("data source=MSI\\SQLEXPRESS;Database=SpenderTracker;trusted_connection=true;TrustServerCertificate=true")
                .EnableDetailedErrors(); 
        }
    } 
}
