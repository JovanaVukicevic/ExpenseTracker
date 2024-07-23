using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAccounts();
        Task<Account?> GetAccountByID(int id);
        Task<List<Account>> GetAllAccountsOfAUser(string id);
        Task<List<Transaction>> GetTransactionsOfATypeOfAccount(int accountId, char indicator);
        Task<Account?> GetAccountByUserIdAndName(string userId, string name);
        Task<bool> AccountExistsId(int id);
        Task<bool> UpdateAccount(Account account);
        Task<bool> DeleteAccount(Account account);
        Task<bool> AddAccount(Account account);
    }
}