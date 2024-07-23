using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
namespace ExpenseTracker.Service.Extensions;
public static class UserExtension
{
    public static User ToUser(this UserDto userDto)
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

    public static UserDto ToDto(this User user)
    {
        var userDto = new UserDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.UserName,
            Password = user.PasswordHash,
            IsPremuium = user.IsPremuium
        };
        return userDto;
    }

    public static UserPublicDisplay ToPublic(this User user)
    {
        var userPublic = new UserPublicDisplay()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.UserName,
            IsPremuium = user.IsPremuium
        };
        return userPublic;
    }
}
