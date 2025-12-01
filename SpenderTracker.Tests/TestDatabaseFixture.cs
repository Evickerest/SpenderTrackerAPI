using Microsoft.EntityFrameworkCore;
using SpenderTracker.Data.Context;
using SpenderTracker.Tests.Model;
using System.Text.Json;

namespace SpenderTracker.Tests;

public class TestDatabaseFixture
{
    private const string ConnectionString = @"data source=MSI\SQLEXPRESS;Database=SpenderTrackerTest;trusted_connection=true;TrustServerCertificate=true";
    private const string SeedDataFilePath = @"Data\SeedData.json";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    SeedData(context); 
                    _databaseInitialized = true; 
                } 
            } 
        } 
    }

    public ApplicationContext CreateContext()
        => new ApplicationContext(
            new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer(ConnectionString)
                .Options);

    public void SeedData(ApplicationContext context)
    {
        string localSeedFilePath = Path.Combine(Directory.GetCurrentDirectory(), SeedDataFilePath); 

        if (!File.Exists(localSeedFilePath))
            throw new FileNotFoundException("Could not find seed data file.");

        string file = File.ReadAllText(localSeedFilePath); 
        SeedData? data = JsonSerializer.Deserialize<SeedData>(file);

        if (data == null)
            throw new ArgumentNullException("Could not deserialize seed data.");

        // Done in the order of child to parent to avoid foreign key issues 
        context.TransactionGroups.AddRange(data.TransactionGroups);
        context.TransactionTypes.AddRange(data.TransactionTypes);
        context.SaveChanges();
        context.Accounts.AddRange(data.Accounts);
        context.Budgets.AddRange(data.Budgets);
        context.SaveChanges();
        context.TransactionMethods.AddRange(data.TransactionMethods);
        context.SaveChanges(); 
        context.Transactions.AddRange(data.Transactions);
        context.SaveChanges(); 
    }
}
