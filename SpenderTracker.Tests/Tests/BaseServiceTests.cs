using Microsoft.EntityFrameworkCore;
using Moq;
using SpenderTracker.Core.Services;
using SpenderTracker.Data.Context;
using SpenderTracker.Data.Dto;
using SpenderTracker.Data.Model;

namespace SpenderTracker.Tests;

public class BaseServiceTest : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture;

    public BaseServiceTest(TestDatabaseFixture fixture)
    {
        Fixture = fixture; 
    } 

    [Theory]
    [InlineData(1, "Checking Account", 2500.75, 1)]
    [InlineData(2, "Savings Account", 10500.00, 2)]
    [InlineData(3, "Business Account", 7800.40, 3)]
    [InlineData(4, "Credit Card", -450.20, 4)]
    [InlineData(5, "Emergency Fund", 3200.00, 5)]
    public async void GetById_ValidAccountId_ReturnsDto(int id, string expectedName, decimal expectedBalance, int expectedId)
    {
        using var context = Fixture.CreateContext();
        var service = new BaseService<Account, AccountDto>(context);

        AccountDto? account = await service.GetById(id, CancellationToken.None);
        Assert.NotNull(account);
        Assert.Equal(expectedId, account.Id);
        Assert.Equal(expectedName, account.AccountName);
        Assert.Equal(expectedBalance, account.Balance); 
    }

    [Theory]
    [InlineData(10)]
    [InlineData(1000)]
    public async void GetById_InvalidAccountId_ReturnsNull(int invalidId)
    {
        using var context = Fixture.CreateContext();
        var service = new BaseService<Account, AccountDto>(context);

        AccountDto? account = await service.GetById(invalidId, CancellationToken.None);
        Assert.Null(account);
    }

    [Fact]
    public async void GetAll_Valid_ReturnsAll()
    {
        using var context = Fixture.CreateContext();
        var service = new BaseService<Account, AccountDto>(context);

        List<AccountDto> accounts = await service.GetAll(CancellationToken.None);
        Assert.Equal(5, accounts.Count); 
    }

    [Theory]
    [InlineData("Test Account 1", 123.45)]
    [InlineData("Test Account 2", 0.0)]
    [InlineData("Test Account 3", -234.34)]
    public async void Insert_ValidEntity_ReturnsDto(string accountName, decimal balance)
    {
        using var context = Fixture.CreateContext();
        context.Database.BeginTransaction();

        var service = new BaseService<Account, AccountDto>(context);
        var newAccount = new AccountDto
        {
            AccountName = accountName,
            Balance = balance 
        };

        var result = await service.Insert(newAccount);
        context.ChangeTracker.Clear();

        Assert.NotNull(result);
        Assert.Equal(accountName, result.AccountName);
        Assert.Equal(balance, result.Balance);
        Assert.True(result.Id > 0); 
    }

    [Theory]
    [InlineData(1, "New Account 1", 2500.75)]
    [InlineData(2, "Savings Account", -42.00)]
    [InlineData(2, "New Account 3", -100.25)]
    public async void Update_ValidEntity_IsSuccessful(int accountId, string newAccountName, decimal newBalance)
    {
        using var context = Fixture.CreateContext();
        context.Database.BeginTransaction();

        var service = new BaseService<Account, AccountDto>(context);
        AccountDto? account = await service.GetById(accountId, CancellationToken.None); 
        Assert.NotNull(account);

        account.AccountName = newAccountName;
        account.Balance = newBalance;

        context.ChangeTracker.Clear();
        var success = await service.Update(account);

        Assert.True(success);

        AccountDto? updatedAccount = await service.GetById(accountId, CancellationToken.None);
        Assert.NotNull(updatedAccount);
        Assert.Equal(accountId, updatedAccount.Id);
        Assert.Equal(newAccountName, updatedAccount.AccountName);
        Assert.Equal(newBalance, updatedAccount.Balance); 
    }

    [Fact]
    public async void Delete_ValidEntity_IsSuccessful()
    {
        using var context = Fixture.CreateContext();
        context.Database.BeginTransaction();

        var service = new BaseService<Account, AccountDto>(context);
        AccountDto? account = await service.GetById(1, CancellationToken.None);
        Assert.NotNull(account);
        context.ChangeTracker.Clear();

        var success = await service.Delete(account.Id);
        Assert.True(success);

        AccountDto? afterAccount = await service.GetById(1, CancellationToken.None);
        Assert.Null(afterAccount); 
    }

    [Fact]
    public async void DoesExist_ValidEntity_DoesExist()
    {
        using var context = Fixture.CreateContext();
        context.Database.BeginTransaction();

        var service = new BaseService<Account, AccountDto>(context);
        bool exists = await service.DoesExist(1, CancellationToken.None);
        Assert.True(exists); 
    }

    [Fact]
    public async void GetById_Cancelled_Throws()
    { 
        var mockContext = new Mock<ApplicationContext>();
        var mockSet = new Mock<DbSet<Account>>(); 

        mockContext.Setup(m => m.Set<Account>()).Returns(mockSet.Object);
        mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>())).Returns(
            async (object[] ids, CancellationToken ct) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1), ct);
                return new Account(); 
            }); 

        var service = new BaseService<Account, AccountDto>(mockContext.Object);
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(20);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => service.GetById(1, cts.Token)); 
    } 
}
