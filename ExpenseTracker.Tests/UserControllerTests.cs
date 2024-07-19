using ExpenseTracker.API.Controllers;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using Moq;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
namespace ExpenseTracker.Tests;
public class UserControllerTests
{
    [Fact]
    public async Task GetAllUsers_ReturnsListOfUsersDto()
    {
        //Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService.Setup(service => service.GetUsersAsync()).Returns(Task.FromResult(new List<UserDto>()));

        var mockAuthenticationService = new Mock<IAuthenticationService>();
        //Act
        var userController = new UserController(mockAuthenticationService.Object, mockUserService.Object);

        //Assert
        var users = await userController.GetAllUsers();
        Assert.NotNull(users);
    }
    [Fact]
    public async Task GetUserById_ReturnsUserDto()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var mockUserDto = new UserDto()
        {
            Email = "   ,",
            FirstName = "sdasd"
        };
        mockUserService.Setup(service => service.GetUserByIDAsync("1")).Returns(Task.FromResult(mockUserDto));
        var mockAuthenticationService = new Mock<IAuthenticationService>();

        // Act
        var userController = new UserController(mockAuthenticationService.Object, mockUserService.Object);

        // Assert
        var actionResult = await userController.GetUserById("1");
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        Assert.Equal(mockUserDto, result.Value);
        Assert.NotEqual((int)HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task RegisterUser_ReturnsTaskActionResultBool()
    {
        //Arrange
        var mockUserService = new Mock<IUserService>();
        var mockUserDto = new UserDto()
        {
            LastName = "sdads",
            IsPremuium = false,
            Password = "sdgghhg1jhghA",
            Email = "rehjhlkjkg",
            FirstName = "adsadss"
        };
        mockUserService.Setup(service => service.RegisterUserAsync(mockUserDto)).Returns(Task.FromResult(Result.Success()));
        var mockAuthenticationService = new Mock<IAuthenticationService>();
        //Act
        var userController = new UserController(mockAuthenticationService.Object, mockUserService.Object);

        //Assert
        var actionResult = await userController.RegisterUser(mockUserDto);
        var result = actionResult.Result as NoContentResult;
        Assert.NotNull(result);
        Assert.NotEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_returnsActionResult()
    {
        var mockHttpContext = new Mock<HttpContext>();
        var mockUserService = new Mock<IUserService>();
        mockUserService.Setup(service => service.DeleteUserAsync("userN1ame")).Returns(Task.FromResult(Result.Success()));
        var mockAuthenticationService = new Mock<IAuthenticationService>();
        var userController = new UserController(mockAuthenticationService.Object, mockUserService.Object);

        //Act
        var actionResult = await userController.DeleteUser("userN1ame");

        //Assert
        Assert.IsType<NoContentResult>(actionResult);
        var result = actionResult as NoContentResult;
        Assert.NotNull(result);
    }

    [Fact]
    public async Task LoginUser_ReturnsTaskActionsResult()
    {
        //Arrange
        var mockUserService = new Mock<IUserService>();
        var mockLoginUser = new LoginUserDto
        {
            Username = "username",
            Password = "shds2hAd"
        };
        var mockAuthenticationService = new Mock<IAuthenticationService>();
        mockAuthenticationService.Setup(service => service.Login(mockLoginUser)).Returns(Task.FromResult(Result.Success<IEnumerable<string>>(["Sdsdsdsdsss"])));
        var userController = new UserController(mockAuthenticationService.Object, mockUserService.Object);
        //Act
        var actionResult = await userController.Login(mockLoginUser);
        //Assert
        Assert.IsType<OkObjectResult>(actionResult);
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);
    }
}