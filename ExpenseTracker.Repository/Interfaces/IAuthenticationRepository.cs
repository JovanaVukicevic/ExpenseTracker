using ExpenseTracker.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Repository.Interfaces;

public interface IAuthenticationRepository
{
    Task<Microsoft.AspNetCore.Identity.IdentityResult> AssignPremiumRoleAsync(User user);
    Task<Microsoft.AspNetCore.Identity.IdentityResult> AssignUserRoleAsync(User user);
    Task<Microsoft.AspNetCore.Identity.IdentityResult> RegisterNewUserAsync(User user, string password);
}