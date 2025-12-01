using SpenderTracker.Core.Services;

namespace SpenderTracker.Tests;

public class TransactionTypeServiceTests : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture;

    public TransactionTypeServiceTests(TestDatabaseFixture fixture)
    { 
        Fixture = fixture;
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [InlineData(4, false)]
    [InlineData(5, false)]
    public async void IsInTransactions_Valid_ReturnsTrue(int typeId, bool expectedIsIn)
    {
        using var context = Fixture.CreateContext();
        var service = new TransactionTypeService(context);

        bool isIn = await service.IsInTransactions(typeId, CancellationToken.None);
        Assert.Equal(expectedIsIn, isIn); 
    }

}
