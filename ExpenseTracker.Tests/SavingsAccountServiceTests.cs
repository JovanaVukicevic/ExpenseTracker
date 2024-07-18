using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Service.Services;
using Moq;

namespace ExpenseTracker.Tests;

public class SavingsAccountServiceTests
{
    private readonly Mock<IUserRepository> mockUserRepository = new();
    private readonly Mock<IAccountRepository> mockAccountRepository = new();
    private readonly Mock<ISavingsAccountRepository> mockSavingsAccountRepository = new();
    private readonly Mock<IScheduledService> mockScheduledService = new();
    private readonly SavingsAccountService savingsAccountService;


    public SavingsAccountServiceTests()
    {
        savingsAccountService = new SavingsAccountService(
            mockScheduledService.Object,
            mockUserRepository.Object,
            mockAccountRepository.Object,
            mockSavingsAccountRepository.Object

        );
    }
    [Fact]
    public async Task GetAllSavingsAccounts_ReturnsTaskListSavingsAccount()
    {
        //Arrange
        mockSavingsAccountRepository.Setup(service => service.GetAllSavingsAccounts()).Returns(Task.FromResult(new List<SavingsAccount>()));

        //Act
        var result = await savingsAccountService.GetAllSAAsync();

        //Assert
        Assert.IsType<List<SavingsAccount>>(result);
    }

    [Fact]
    public async Task UpdateSavingsAccount_ReturnsTaskBool()
    {
        //Arrange
        var savingsAccount = new SavingsAccount
        {
            Name = "lalala"
        };

        mockSavingsAccountRepository.Setup(service => service.UpdateSavingsAccount(savingsAccount)).Returns(Task.FromResult(true));

        //Act
        var result = await savingsAccountService.UpdateSavingsAccount(savingsAccount);

        //Assert
        Assert.True(result);

    }

    [Fact]
    public async Task RemoveSavingsAccount_ReturnsTaskResultIEnumerable()
    {
        //Arrange
        var savingsAccount = new SavingsAccount
        {
            Name = "lalala"
        };
        var user = new User
        {
            UserName = "username"
        };

        mockUserRepository.Setup(service => service.GetUserByUsername(user.UserName)).Returns(Task.FromResult(user));
        mockSavingsAccountRepository.Setup(service => service.GetSAccountsOfAUser(user.UserName)).Returns(Task.FromResult(savingsAccount));
        mockSavingsAccountRepository.Setup(service => service.DeleteSavingsAccount(savingsAccount)).Returns(Task.FromResult(true));

        //Act
        var result = await savingsAccountService.RemoveSAccount(user.UserName);

        //Assert
        Assert.IsType<Result<SavingsAccountDto, IEnumerable<string>>>(result);

    }

    [Fact]
    public async Task CreateSavingsTraansaction_ReturnsTaskBool()
    {
        //Arrange
        var savingsAccount = new SavingsAccount
        {
            Name = "lalala"
        };
        var user = new User
        {
            UserName = "username"
        };
        var scheduledSavingsTransaction = new ScheduledDto
        {
            AccountID = 1
        };

        var savingsDto = new SavingsAccountDto { Name = "sdasda" };
        var account = new Account { ID = 1 };
        mockSavingsAccountRepository.Setup(service => service.GetSAccountsOfAUser("1")).Returns(Task.FromResult(savingsAccount));
        mockScheduledService.Setup(service => service.CreateScheduledExpenseAsync(scheduledSavingsTransaction)).Returns(Task.FromResult(Result.Success()));

        //Act
        var result = await savingsAccountService.CreateSavingsTransactions("1", account, savingsDto);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetSavingsAccountById_ReturnsTaskSavingsAccount()
    {
        //Arrange
        var savingsAccount = new SavingsAccount
        {
            Name = "lalala"
        };

        mockSavingsAccountRepository.Setup(service => service.GetSAccountByID(1)).Returns(Task.FromResult(savingsAccount));

        //Act
        var result = await savingsAccountService.GetSavingsAccountByID(1);

        //Assert
        Assert.Equal(savingsAccount, result);

    }

    [Fact]
    public async Task CreateSavingsAccount_ReturnsTaskResult()
    {
        //Arrange
        var user = new User
        {
            UserName = "username"
        };
        var account = new Account
        {
            ID = 1,
            Name = "username"
        };
        var savingsAccount = new SavingsAccount
        {
            ID = 1,
            Name = "lalala"
        };
        var scheduledSavingsTransaction = new ScheduledDto
        {
            AccountID = 1
        };

        mockUserRepository.Setup(service => service.GetUserByUsername(user.UserName)).Returns(Task.FromResult(user));
        mockAccountRepository.Setup(service => service.GetAccountByUserIdAndName("1", "Name")).Returns(Task.FromResult(account));
        mockSavingsAccountRepository.Setup(service => service.CreateSAccount(savingsAccount)).Returns(Task.FromResult(true));
        mockSavingsAccountRepository.Setup(service => service.GetSAccountsOfAUser("1")).Returns(Task.FromResult(savingsAccount));
        mockScheduledService.Setup(service => service.CreateScheduledExpenseAsync(scheduledSavingsTransaction)).Returns(Task.FromResult(Result.Success()));
        mockAccountRepository.Setup(service => service.UpdateAccount(account)).Returns(Task.FromResult(true));

        //Act
        var result = await savingsAccountService.CreateSavingsAccount(savingsAccount.ToDto(), user.UserName, account.Name);

        //Assert
        Assert.IsType<Result<SavingsAccountDto, IEnumerable<string>>>(result);
    }
}