using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetUserById(string id);

    public Task<List<User>> GetAllUsers();

    public Task<User?> GetUserByUsernameAndPassword(string username, string password);

    public Task<bool> CreateUser(User user);

    public Task<bool> DeleteUser(User user);

    public Task<bool> UpdateUser(User user);

    public Task<User?> GetUserByUsername(string username);

    public Task<bool> UserExists(string username);

    public Task<List<User>> GetAllPremiumUsers();

}