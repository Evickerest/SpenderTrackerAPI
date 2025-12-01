using SpenderTracker.Core.Services;

namespace SpenderTracker.Tests;

public class AccountServiceTests : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture;

    public AccountServiceTests(TestDatabaseFixture fixture)
    {
        Fixture = fixture;
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [InlineData(4, true)]
    [InlineData(5, true)]
    public async void IsInTransactions_Valid_ReturnsTrue(int accountId, bool expectedIsIn)
    {
        using var context = Fixture.CreateContext();
        var service = new AccountService(context);

        bool isIn = await service.IsInTransactions(accountId, CancellationToken.None);
        Assert.Equal(expectedIsIn, isIn); 
    }
}

