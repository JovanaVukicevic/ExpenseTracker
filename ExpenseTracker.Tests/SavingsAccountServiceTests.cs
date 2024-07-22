using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.CustomException;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Service.Services;
using Moq;

namespace ExpenseTracker.Tests;

public class SavingsAccountServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly Mock<IAccountRepository> _mockAccountRepository = new();
    private readonly Mock<ISavingsAccountRepository> _mockSavingsAccountRepository = new();
    private readonly Mock<IScheduledService> _mockScheduledService = new();
    private readonly SavingsAccountService savingsAccountService;


    public SavingsAccountServiceTests()
    {
        savingsAccountService = new SavingsAccountService(
            _mockScheduledService.Object,
            _mockUserRepository.Object,
            _mockAccountRepository.Object,
            _mockSavingsAccountRepository.Object

        );
    }
    [Fact]
    public async Task GetAllSavingsAccounts_ReturnsTaskListSavingsAccount()
    {
        //Arrange
        _mockSavingsAccountRepository.Setup(service => service.GetAllSavingsAccounts()).Returns(Task.FromResult(new List<SavingsAccount>()));

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

        _mockSavingsAccountRepository.Setup(service => service.UpdateSavingsAccount(savingsAccount)).Returns(Task.FromResult(true));

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
            Id = "asd",
            UserName = "username"
        };

        _mockUserRepository.Setup(service => service.GetUserByUsername(user.UserName)).Returns(Task.FromResult(user));
        _mockSavingsAccountRepository.Setup(service => service.GetSAccountsOfAUser(user.Id)).Returns(Task.FromResult(savingsAccount));
        _mockSavingsAccountRepository.Setup(service => service.DeleteSavingsAccount(savingsAccount)).Returns(Task.FromResult(true));

        //Act
        var result = await savingsAccountService.RemoveSAccount(user.UserName);

        //Assert
        Assert.IsType<Result<SavingsAccountDto, string>>(result);

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
            Id = "asda",
            UserName = "username"
        };
        var scheduledSavingsTransaction = new ScheduledDto
        {
            AccountID = 1
        };

        var savingsDto = new SavingsAccountDto { Name = "sdasda" };
        var account = new Account { ID = 1 };
        _mockUserRepository.Setup(service => service.GetUserById(user.Id)).Returns(Task.FromResult(user));
        _mockSavingsAccountRepository.Setup(service => service.GetSAccountsOfAUser(user.Id)).Returns(Task.FromResult(savingsAccount));
        _mockScheduledService.Setup(service => service.CreateScheduledExpenseAsync(scheduledSavingsTransaction, "bcxbvsdv")).Returns(Task.FromResult(Result.Success()));
        _mockAccountRepository.Setup(service => service.UpdateAccount(account)).Returns(Task.FromResult(true));
        //Act
        var result = await savingsAccountService.CreateSavingsTransactions(user.Id, account, savingsDto);

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

        _mockSavingsAccountRepository.Setup(service => service.GetSAccountByID(1)).Returns(Task.FromResult(savingsAccount));

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
            Id = "sada",
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

        _mockUserRepository.Setup(service => service.GetUserByUsername(user.UserName)).Returns(Task.FromResult(user));
        _mockAccountRepository.Setup(service => service.GetAccountByUserIdAndName("1", "Name")).Returns(Task.FromResult(account));
        _mockSavingsAccountRepository.Setup(service => service.CreateSAccount(savingsAccount)).Returns(Task.FromResult(true));
        _mockSavingsAccountRepository.Setup(service => service.GetSAccountsOfAUser(user.Id)).Returns(Task.FromResult(savingsAccount));
        _mockScheduledService.Setup(service => service.CreateScheduledExpenseAsync(scheduledSavingsTransaction, user.UserName)).Returns(Task.FromResult(Result.Success()));
        _mockAccountRepository.Setup(service => service.UpdateAccount(account)).Returns(Task.FromResult(true));

        //Act
        var result = await savingsAccountService.CreateSavingsAccount(savingsAccount.ToDto(), user.UserName, account.Name);

        //Assert
        Assert.IsType<Result<SavingsAccountDto, string>>(result);
    }
}