using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Interfaces
{

    public interface IAccountService
    {
        public Task<Result<AccountDto, IEnumerable<string>>> CreateAccount(AccountDto a, string username);
        public Task<Result<AccountDto, IEnumerable<string>>> RemoveAccount(string name, string username);

        public Task<Account> GetAccountByID(int accountId);

        public Task<bool> UpdateAccountAsync(AccountDto accountDto);

        public Task<List<Account>> GetAllAccountsOfAUser(string id);
    }
}
