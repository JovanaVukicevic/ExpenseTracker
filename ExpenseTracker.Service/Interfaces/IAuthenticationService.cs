using CSharpFunctionalExtensions;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Interfaces
{

    public interface IAuthenticationService
    {
        Task<Result<UserDto, IEnumerable<string>>> RegisterUser(UserDto user);
        Task<Result<IEnumerable<string>>> Login(LoginUserDto loginUser);
        Task<string> GenerateTokenString(LoginUserDto loginUser);
    }
}