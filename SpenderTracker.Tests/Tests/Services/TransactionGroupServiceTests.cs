using SpenderTracker.Core.Services;

namespace SpenderTracker.Tests;

public class TransactionGroupServiceTests : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture; 

    public TransactionGroupServiceTests(TestDatabaseFixture fixture)
    { 
        Fixture = fixture;
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, true)]
    [InlineData(5, false)]
    public async void IsInTransactions_Valid_ReturnsTrue(int groupId, bool expectedIsIn)
    {
        using var context = Fixture.CreateContext();
        var service = new TransactionGroupService(context);

        bool isIn = await service.IsInTransactions(groupId, CancellationToken.None);
        Assert.Equal(expectedIsIn, isIn); 
    }
}

