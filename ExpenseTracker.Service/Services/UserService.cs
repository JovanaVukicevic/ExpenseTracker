using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
//using ExpenseTracker.Repository.Repository;
using ExpenseTracker.Repository.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

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
    public User FromDtoToUser(UserDto userDto)
    {
        var user = new User()
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            UserName = userDto.Username,
            PasswordHash = userDto.Password,
            IsPremuium = userDto.IsPremuium,
            SavingsAccountID = 0,
        };
        return user;
    }

    public UserDto FromUserToDto(User user)
    {
        var userDto = new UserDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.UserName,
            Password = user.PasswordHash,
            IsPremuium = user.IsPremuium
        };

        return userDto;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        List<User> users = await _userRepository.GetAllUsers();
        if (users == null)
        {
            return null;
        }
        List<UserDto> usersDto = [];
        foreach (User user in users)
        {
            usersDto.Add(FromUserToDto(user));
        }
        return usersDto;

    }

    public async Task<UserDto> GetUserByIDAsync(string userId)
    {
        User user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            return null;
        }
        return FromUserToDto(user);
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
        User user = FromDtoToUser(userDto);
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
        var user = await _userRepository.GetUserByUsername(username);
        var result = await _userRepository.DeleteUser(user);
        if (result != true)
        {
            return Result.Failure<string>("Something went wrong while deleting a user");
        }
        return Result.Success<string>("User is deleted!");

    }
}