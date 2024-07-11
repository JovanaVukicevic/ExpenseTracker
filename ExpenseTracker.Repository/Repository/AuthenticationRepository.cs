using ExpenseTracker.Repository.Constants;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Repository;
using ExpenseTracker.Repository.Data;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Repository.Repository;

public class AuthenticationRepository : ExpenseTracker.Repository.Interfaces.IAuthenticationRepository
{

    private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;


    public AuthenticationRepository(Microsoft.AspNetCore.Identity.UserManager<User> userManager)
    {
        _userManager = userManager;

    }

    public async Task<IdentityResult> AssignPremiumRoleAsync(User user)
    {
        Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.AddToRoleAsync(user, Roles.Premium);
        return result;
    }

    public async Task<IdentityResult> AssignUserRoleAsync(User user)
    {
        Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.AddToRoleAsync(user, Roles.User);
        return result;
    }

    public async Task<Microsoft.AspNetCore.Identity.IdentityResult> RegisterNewUserAsync(User user, string password)
    {
        Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.CreateAsync(user, password);
        return result;
    }
}