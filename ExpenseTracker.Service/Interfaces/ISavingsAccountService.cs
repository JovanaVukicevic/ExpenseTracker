using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using Microsoft.Net.Http.Headers;

namespace ExpenseTracker.Service.Interfaces
{

    public interface ISavingsAccountService
    {
        Task<Result<SavingsAccountDto, IEnumerable<string>>> CreateSavingsAccount(SavingsAccountDto a, string username, string AccountName);

        public Task<Result<SavingsAccountDto, IEnumerable<string>>> RemoveSAccount(string username);

        public SavingsAccount FromDtoToSavingsAccount(SavingsAccountDto savingsAccountDto);

        public SavingsAccountDto FromSAToDto(SavingsAccount savingsAccount);

        public Task<List<SavingsAccount>> GetAllSAAsync();


    }
}