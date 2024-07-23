using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface ISavingsAccountRepository
    {
        Task<List<SavingsAccount>> GetAllSavingsAccounts();
        Task<SavingsAccount?> GetSAccountByID(int id);
        Task<bool> CreateSAccount(SavingsAccount savingsAccount);
        Task<bool> UpdateSavingsAccount(SavingsAccount savingsAccount);
        Task<bool> DeleteSavingsAccount(SavingsAccount savingsAccount);
        Task<SavingsAccount?> GetSAccountsOfAUser(string userId);
    }
}
