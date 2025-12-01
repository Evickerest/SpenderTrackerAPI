using SpenderTracker.Core.Services;

namespace SpenderTracker.Tests;

public class TransactionServiceTests : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture; 

    public TransactionServiceTests(TestDatabaseFixture fixture)
    { 
        Fixture = fixture;
    }

    [Theory]
    [InlineData(null, null, null, null, 5)]
    [InlineData(1, null, null, null, 4)]
    [InlineData(null, 2, null, null, 1)]
    [InlineData(null, null, 3, null, 2)]
    [InlineData(null, null, null, 4, 1)]
    [InlineData(1, 2, null, null, 1)]
    [InlineData(1, null, 3, null, 2)]
    [InlineData(null, 2, null, 4, 0)]
    [InlineData(1, 1, 1, 1, 1)]
    public async void GetAll_ValidIds_ReturnsCount(int? typeId, int? groupId, int? methodId, int? accountId, int expectedCount)
    {
        using var context = Fixture.CreateContext();
        var service = new TransactionService(context);

        var results = await service.GetAll(typeId, groupId, methodId, accountId, CancellationToken.None);
        Assert.Equal(expectedCount, results.Count); 
    } 
}
