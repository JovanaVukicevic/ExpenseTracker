using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Extensions;
using CSharpFunctionalExtensions;
using ExpenseTracker.Service.CustomException;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<UserService> _logger;
    private readonly IAccountService _accountService;

    public UserService(IUserRepository userRepository, IAccountService accountService, IAuthenticationService authService, IMemoryCache cache, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _authenticationService = authService;
        _logger = logger;
        _cache = cache;
        _accountService = accountService;
    }
    public async Task<List<UserPublicDisplay>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllUsers();
        List<UserPublicDisplay> usersPublic = [];
        foreach (User user in users)
        {
            usersPublic.Add(user.ToPublic());
        }

        return usersPublic;
    }

    public async Task<UserDto> GetUserByIDAsync(string userId)
    {
        var cacheKey = $"UserDto-{userId}";
        if (!_cache.TryGetValue(cacheKey, out UserDto? result))
        {
            _logger.LogInformation("Getting user from database because it doesn't exist in cache.");
            var user = (await _userRepository.GetUserById(userId))
                ?? throw new NotFoundException("User not found");

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            _logger.LogInformation("Putting user in cache.");
            result = user.ToDto();
            _cache.Set(cacheKey, result, cacheEntryOptions);
        }

        return result
            ?? throw new NotFoundException($"User with ID {userId} not found.");
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

        var result = await _authenticationService.RegisterUser(userDto);
        if (!result.IsSuccess)
        {
            return Result.Failure<string>("Error while registering a user");
        }

        var newAccount = new AccountDto { Name = "Default account" };
        var createAccount = await _accountService.CreateAccount(newAccount, userDto.Username);
        if (!createAccount.IsSuccess)
        {
            return Result.Failure<string>("Something went wrong while creating default account");
        }

        return Result.Success<string>("Registration was succesfull!");
    }

    public async Task<Result> DeleteUserAsync(string username)
    {
        if (!await _userRepository.UserExists(username))
        {
            return Result.Failure<string>("User doesn't exist!");
        }

        var user = await _userRepository.GetUserByUsername(username)
            ?? throw new NotFoundException("User not found");

        var result = await _userRepository.DeleteUser(user);
        if (result != true)
        {
            return Result.Failure<string>("Something went wrong while deleting a user");
        }

        return Result.Success<string>("User is deleted!");
    }

    public async Task<List<User>> GetAllPremiumUsersAsync()
    {
        return await _userRepository.GetAllPremiumUsers();
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _userRepository.GetUserByUsername(username)
            ?? throw new NotFoundException($"User with username {username} was not found.");
    }
}
