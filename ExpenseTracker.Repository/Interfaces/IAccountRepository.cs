using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAccounts();
        Task<Account> GetAccountByID(int id);
        Task<List<Account>> GetAllAccountsOfAUser(string username);
        Task<User> GetOwnerOfTheAccount(int accountId);
        Task<List<Transaction>> GetTransactionsOfATypeOfAccount(int accountId, char c);
        Task<List<Transaction>> GetTransactionsOfACategoryOfAccount(int accountId, string name);

        Task<Account> GetAccountByUserIdAndName(string userId, string name);
        Task<bool> AccountExistsId(int id);
        Task<bool> UpdateAccount(Account a);
        Task<bool> DeleteAccount(Account a);
        Task<bool> AddAccount(Account a);


    }
}