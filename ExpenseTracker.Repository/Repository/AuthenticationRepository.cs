using ExpenseTracker.Repository.Constants;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Repository.Repository;

public class AuthenticationRepository : IAuthenticationRepository
{

    private readonly UserManager<User> _userManager;


    public AuthenticationRepository(UserManager<User> userManager)
    {
        _userManager = userManager;

    }

    public async Task<IdentityResult> AssignPremiumRoleAsync(User user)
    {
        IdentityResult result = await _userManager.AddToRoleAsync(user, Roles.Premium);
        return result;
    }

    public async Task<IdentityResult> AssignUserRoleAsync(User user)
    {
        IdentityResult result = await _userManager.AddToRoleAsync(user, Roles.User);
        return result;
    }

    public async Task<IdentityResult> RegisterNewUserAsync(User user, string password)
    {
        IdentityResult result = await _userManager.CreateAsync(user, password);
        return result;
    }
}