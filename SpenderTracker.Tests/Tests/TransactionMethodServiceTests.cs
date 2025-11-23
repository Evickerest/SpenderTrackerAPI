using SpenderTracker.Core.Services;

namespace SpenderTracker.Tests;

public class TransactionMethodServiceTests : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture;

    public TransactionMethodServiceTests(TestDatabaseFixture fixture)
    {
        Fixture = fixture;
    } 

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 1)]
    [InlineData(5, 0)]
    public void GetAllByAccountId_ValidAccountId_ReturnsAll(int accountId, int expectedCount) 
    {
        using var context = Fixture.CreateContext();
        var service = new TransactionMethodService(context);

        var methods = service.GetAllByAccountId(accountId);
        Assert.Equal(expectedCount, methods.Count); 
    }
} 