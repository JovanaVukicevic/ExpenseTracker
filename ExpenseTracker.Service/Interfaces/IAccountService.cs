using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Interfaces
{

    public interface IAccountService
    {
        public Task<Result<AccountDto, IEnumerable<string>>> CreateAccount(AccountDto a, string username);
        public Task<Result<AccountDto, IEnumerable<string>>> RemoveAccount(string name, string username);

        public Account FromDtoToAccount(AccountDto accountDto);

        public AccountDto FromAccountToDto(Account account);
    }
}