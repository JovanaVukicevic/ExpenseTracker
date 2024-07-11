

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Repository;
using ExpenseTracker.Repository.Models;
using static ExpenseTracker.Service.Dto.UserDto;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;



namespace ExpenseTracker.Service.Services;

public class AuthenticationService : IAuthenticationService
{
    public readonly IUserRepository _userRepository;

    private readonly IAuthenticationRepository _authenticationRepository;
    public readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _config;

    private readonly UserManager<User> _userManager;

    // private readonly IUserService _userService;
    public AuthenticationService(IUserRepository userRepository, UserManager<User> userManager, IConfiguration config, SignInManager<User> signInManager, IAuthenticationRepository authenticationRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _authenticationRepository = authenticationRepository;
        //_userService = userService;
    }

    public async Task<string> GenerateTokenString(LoginUserDto loginUser)
    {
        var user = await _userRepository.GetUserByUsername(loginUser.Username);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginUser.Username),
                new Claim(ClaimTypes.Name, loginUser.Username),
            };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
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

    public async Task<Result<IEnumerable<string>>> Login(LoginUserDto loginUser)
    {
        var exist = await _userRepository.GetUserByUsername(loginUser.Username);
        if (exist == null)
        {
            return Result.Failure<IEnumerable<string>>("There is no user with :" + loginUser.Username + " username.");
        }

        var result = await _signInManager.PasswordSignInAsync(loginUser.Username, loginUser.Password, false, false);

        if (result.Succeeded)
        {
            string tokenString = await GenerateTokenString(loginUser);
            return Result.Success<IEnumerable<string>>(new List<string> { tokenString });
        }
        return Result.Failure<IEnumerable<string>>("Login attempt was unsuccessful. Please try again!");

    }

    public async Task<Result<UserDto, IEnumerable<string>>> RegisterUser(UserDto user)
    {
        var identityUser = FromDtoToUser(user);

        var resultUser = await _authenticationRepository.RegisterNewUserAsync(identityUser, user.Password);

        if (resultUser.Succeeded)
        {
            var resultRole = await _authenticationRepository.AssignUserRoleAsync(identityUser);
            if (!resultRole.Succeeded)
            {
                return Result.Failure<UserDto, IEnumerable<string>>(resultUser.Errors.Select(e => e.Description));
            }
            return Result.Success<UserDto, IEnumerable<string>>(user);
        }
        return Result.Failure<UserDto, IEnumerable<string>>(resultUser.Errors.Select(e => e.Description));

    }


}