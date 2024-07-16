using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Extensions;

public static class SavingsAccountExtension
{
    public static SavingsAccount ToSavingsAccount(this SavingsAccountDto savingsAccountDto)
    {
        var savingsAccount = new SavingsAccount
        {
            Name = savingsAccountDto.Name,
            TargetDate = savingsAccountDto.TargetDate,
            TargetAmount = savingsAccountDto.TargetAmount,
            AmountPerMonth = savingsAccountDto.AmountPerMonth
        };
        return savingsAccount;
    }

    public static SavingsAccountDto ToDto(this SavingsAccount savingsAccount)
    {
        var savingsAccountDto = new SavingsAccountDto
        {
            Name = savingsAccount.Name,
            TargetAmount = savingsAccount.TargetAmount,
            AmountPerMonth = savingsAccount.AmountPerMonth
        };
        return savingsAccountDto;
    }
}
