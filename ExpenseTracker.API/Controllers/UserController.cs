using ExpenseTracker.Repository.Constants;
using ExpenseTracker.Service.CustomException;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;

    public UserController(IAuthenticationService authenticationService, IUserService userService)
    {
        _authenticationService = authenticationService;
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllUsers()
    {
        var result = await _userService.GetUsersAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public async Task<ActionResult<UserDto>> GetUserById([FromRoute] string id)
    {
        var user = await _userService.GetUserByIDAsync(id);
        if (user == null)
        {
            return NotFound("User not found");
        }
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public async Task<ActionResult<UserDto>> RegisterUser([FromBody] UserDto userDto)
    {
        var result = await _userService.RegisterUserAsync(userDto);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    [Authorize(Roles = Roles.User)]
    public async Task<ActionResult> DeleteUser(string username)
    {
        string usernameClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name)
             ?? throw new NotFoundException("User not found");
        if (usernameClaim != username)
        {
            return Unauthorized();
        }
        var result = await _userService.DeleteUserAsync(username);
        if (result.IsFailure)
        {
            return BadRequest("Bad request");
        }
        return NoContent();
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public async Task<ActionResult> Login(LoginUserDto loginUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var result = await _authenticationService.Login(loginUser);
        if (result.IsFailure)
        {
            return Unauthorized();
        }
        return Ok(result.Value);
    }
}
