using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using CSharpFunctionalExtensions;

namespace ExpenseTracker.Service.Interfaces
{

    public interface IUserService
    {
        public Task<List<UserPublicDisplay>> GetUsersAsync();

        public Task<UserDto> GetUserByIDAsync(string userId);

        public Task<Result> RegisterUserAsync(UserDto userDto);

        public Task<Result> DeleteUserAsync(string username);
        public Task<List<User>> GetAllPremiumUsersAsync();

        public Task<User> GetUserByUsernameAsync(string username);
    }
}