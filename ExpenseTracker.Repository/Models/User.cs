using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace ExpenseTracker.Repository.Models;


public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Boolean IsPremuium { get; set; }
    public List<Account> Accounts { get; set; } = null!;
    public int SavingsAccountID { get; set; }

    public List<Category> Categories { get; set; } = null!;
}