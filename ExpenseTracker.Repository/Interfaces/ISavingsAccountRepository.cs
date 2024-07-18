using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface ISavingsAccountRepository
    {
        Task<List<SavingsAccount>> GetAllSavingsAccounts();
        Task<SavingsAccount?> GetSAccountByID(int id);
        Task<bool> CreateSAccount(SavingsAccount sa);
        Task<bool> UpdateSavingsAccount(SavingsAccount sa);
        Task<bool> DeleteSavingsAccount(SavingsAccount sa);
        Task<SavingsAccount?> GetSAccountsOfAUser(string userId);
    }
}
