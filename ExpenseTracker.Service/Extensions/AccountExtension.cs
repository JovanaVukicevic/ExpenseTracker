using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Extensions;

public static class AccountExtension
{
    public static Account ToAccount(this AccountDto accountDto)
    {
        var account = new Account
        {
            Name = accountDto.Name,
            Date = DateTime.Today,
            Balance = 0,
            SavingsAccountID = 0,
        };
        return account;
    }
    public static AccountDto ToDto(this Account account)
    {
        var accountDto = new AccountDto
        {
            Name = account.Name,
        };
        return accountDto;
    }
}
