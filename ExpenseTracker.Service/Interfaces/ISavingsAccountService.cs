using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Interfaces
{

    public interface ISavingsAccountService
    {
        Task<Result<SavingsAccountDto, string>> CreateSavingsAccount(SavingsAccountDto a, string username, string AccountName);

        public Task<Result<SavingsAccountDto, string>> RemoveSAccount(string username);
        public Task<List<SavingsAccount>> GetAllSAAsync();

        public Task<bool> UpdateSavingsAccount(SavingsAccount savingsAccount);

        public Task<SavingsAccount> GetSavingsAccountByID(int id);

        public Task<bool> CreateSavingsTransactions(string userId, Account account, SavingsAccountDto savingsAccountDto);


    }
}
