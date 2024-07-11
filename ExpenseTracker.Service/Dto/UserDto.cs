using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Service.Dto;

public class UserDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public Boolean IsPremuium { get; set; }
    public static User FromDtoToUser(UserDto userDto)
    {
        var user = new User()
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            UserName = userDto.Username,
            PasswordHash = userDto.Password,
            IsPremuium = userDto.IsPremuium,
            SavingsAccountID = 0,
        };
        return user;
    }

}