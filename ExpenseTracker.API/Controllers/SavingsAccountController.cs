using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Constants;


namespace ExpenseTracker.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SavingsAccountController : ControllerBase
{
    private readonly ISavingsAccountService _savingsService;
    private readonly IAuthenticationService _authService;

    private readonly IUserService _userService;
    public SavingsAccountController(ISavingsAccountService savingsService, IAuthenticationService authService, IUserService userService)
    {
        _savingsService = savingsService;
        _authService = authService;
        _userService = userService;

    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<SavingsAccount>> GetAllSavingsAccounts()
    {
        var result = await _savingsService.GetAllSAAsync();
        return result;
    }

    [HttpPost("Plan")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<SavingsAccountDto> CreateSavingsPlan(SavingsAccountDto savingsAccountDto)
    {
        double amountPerMonth = savingsAccountDto.TargetAmount / (savingsAccountDto.TargetDate.Month - DateTime.Now.Month + 1);
        savingsAccountDto.AmountPerMonth = amountPerMonth;
        return savingsAccountDto;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateSAccount(SavingsAccountDto savingsAccount, string accountName)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        //var user = _userRepository.GetUserByUsername(username);
        var result = await _savingsService.CreateSavingsAccount(savingsAccount, username, accountName);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);

    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = Roles.User)]
    public async Task<ActionResult> DeleteAnAccount()
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var result = await _savingsService.RemoveSAccount(username);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);

    }
}
