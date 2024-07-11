using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using CSharpFunctionalExtensions;

namespace ExpenseTracker.Service.Interfaces
{

    public interface IUserService
    {
        public User FromDtoToUser(UserDto userDto);

        public UserDto FromUserToDto(User user);

        public Task<List<UserDto>> GetUsersAsync();

        public Task<UserDto> GetUserByIDAsync(string userId);

        public Task<Result> RegisterUserAsync(UserDto user);

        public Task<Result> DeleteUserAsync(string username);
    }
}