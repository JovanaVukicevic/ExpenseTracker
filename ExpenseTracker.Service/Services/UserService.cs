using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Extensions;
using CSharpFunctionalExtensions;
using ExpenseTracker.Service.CustomException;

namespace ExpenseTracker.Service.Services;

public class UserService : IUserService
{
    public readonly IUserRepository _userRepository;

    public readonly IAuthenticationService _authService;

    public UserService(IUserRepository userRepository, IAuthenticationService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }
    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllUsers()
            ?? throw new NotFoundException("Users not found.");
        List<UserDto> usersDto = [];
        foreach (User user in users)
        {
            usersDto.Add(user.ToDto());
        }
        return usersDto;

    }

    public async Task<UserDto> GetUserByIDAsync(string userId)
    {
        var user = await _userRepository.GetUserById(userId)
            ?? throw new NotFoundException($"User with ID {userId} not found.");
        return user.ToDto();
    }

    public async Task<Result> RegisterUserAsync(UserDto userDto)
    {
        if (userDto == null)
        {
            return Result.Failure<string>("Error while registering a user");
        }
        var exist = await _userRepository.GetUserByUsername(userDto.Username);
        if (exist != null && exist.UserName == userDto.Username)
        {
            return Result.Failure<string>("Username is already taken");
        }
        User user = userDto.ToUser();
        var result = await _authService.RegisterUser(userDto);
        if (!result.IsSuccess)
        {
            return Result.Failure<string>("Error while registering a user");
        }
        return Result.Success<string>("Registration was succesfull!");
    }

    public async Task<Result> DeleteUserAsync(string username)
    {
        if (!await _userRepository.UserExists(username))
        {
            return Result.Failure<string>("User doesn't exist!");
        }
        var user = await _userRepository.GetUserByUsername(username) ?? throw new NotFoundException("User not found");
        var result = await _userRepository.DeleteUser(user);
        if (result != true)
        {
            return Result.Failure<string>("Something went wrong while deleting a user");
        }
        return Result.Success<string>("User is deleted!");

    }

    public async Task<List<User>> GetAllPremiumUsersAsync()
    {
        var result = await _userRepository.GetAllPremiumUsers() ?? throw new NotFoundException("Users not found");
        return result;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var result = await _userRepository.GetUserByUsername(username) ?? throw new NotFoundException($"User with username {username} was not found.");
        return result;
    }
}
