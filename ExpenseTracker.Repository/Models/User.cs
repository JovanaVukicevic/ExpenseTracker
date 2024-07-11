using Microsoft.AspNetCore.Identity;
namespace ExpenseTracker.Repository.Models;


public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Boolean IsPremuium { get; set; }
    public List<Account> Accounts { get; set; }
    public int SavingsAccountID { get; set; }
}