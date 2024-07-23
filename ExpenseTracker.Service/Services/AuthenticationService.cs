using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.CustomException;

namespace ExpenseTracker.Service.Services;

public class AuthenticationService : IAuthenticationService
{
    public readonly IUserRepository _userRepository;
    private readonly IAuthenticationRepository _authenticationRepository;
    public readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;

    public AuthenticationService(IUserRepository userRepository, UserManager<User> userManager, IConfiguration config, SignInManager<User> signInManager, IAuthenticationRepository authenticationRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _authenticationRepository = authenticationRepository;
    }

    public async Task<string> GenerateTokenString(LoginUserDto loginUserDto)
    {
        var user = await _userRepository.GetUserByUsername(loginUserDto.Username)
            ?? throw new NotFoundException("Account not found");
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginUserDto.Username),
                new Claim(ClaimTypes.Name, loginUserDto.Username),
            };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

#pragma warning disable CS8604 // Possible null reference argument.
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
#pragma warning restore CS8604 // Possible null reference argument.
        var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            issuer: _config.GetSection("Jwt:Issuer").Value,
            audience: _config.GetSection("Jwt:Audience").Value,
            signingCredentials: signingCred
            );
        string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return tokenString;
    }

    public async Task<Result<IEnumerable<string>>> Login(LoginUserDto loginUserDto)
    {
        var exist = await _userRepository.GetUserByUsername(loginUserDto.Username);
        if (exist == null)
        {
            return Result.Failure<IEnumerable<string>>($"There is no user with : {loginUserDto.Username} username.");
        }

        var result = await _signInManager.PasswordSignInAsync(loginUserDto.Username, loginUserDto.Password, false, false);

        if (result.Succeeded)
        {
            string tokenString = await GenerateTokenString(loginUserDto);
            return Result.Success<IEnumerable<string>>(new List<string> { tokenString });
        }
        return Result.Failure<IEnumerable<string>>("Login attempt was unsuccessful. Please try again!");

    }

    public async Task<Result<UserDto, IEnumerable<string>>> RegisterUser(UserDto userDto)
    {
        var identityUser = userDto.ToUser();

        var resultUser = await _authenticationRepository.RegisterNewUserAsync(identityUser, userDto.Password);

        if (!resultUser.Succeeded)
        {
            return Result.Failure<UserDto, IEnumerable<string>>(resultUser.Errors.Select(e => e.Description));
        }
        var resultRole = await _authenticationRepository.AssignUserRoleAsync(identityUser);
        if (!resultRole.Succeeded)
        {
            return Result.Failure<UserDto, IEnumerable<string>>(resultUser.Errors.Select(e => e.Description));
        }
        return Result.Success<UserDto, IEnumerable<string>>(userDto);
    }
}