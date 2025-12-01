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
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    [InlineData(5, true)]
    public async void IsInTransactions_Valid_ReturnsTrue(int methodId, bool expectedIsIn)
    {
        using var context = Fixture.CreateContext();
        var service = new TransactionMethodService(context);

        bool isIn = await service.IsInTransactions(methodId, CancellationToken.None);
        Assert.Equal(expectedIsIn, isIn); 
    }
}
