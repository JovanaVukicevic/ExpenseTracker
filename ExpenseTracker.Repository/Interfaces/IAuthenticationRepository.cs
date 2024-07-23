using ExpenseTracker.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Repository.Interfaces;
public interface IAuthenticationRepository
{
    Task<IdentityResult> AssignPremiumRoleAsync(User user);
    Task<IdentityResult> AssignUserRoleAsync(User user);
    Task<IdentityResult> RegisterNewUserAsync(User user, string password);
}