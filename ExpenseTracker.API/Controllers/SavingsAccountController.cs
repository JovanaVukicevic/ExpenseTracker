using System.Security.Claims;
using ExpenseTracker.Repository.Constants;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(CustomExceptionFilter))]
public class SavingsAccountController : ControllerBase
{
    private readonly ISavingsAccountService _savingsService;
    public SavingsAccountController(ISavingsAccountService savingsService)
    {
        _savingsService = savingsService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllSavingsAccounts()
    {
        var result = await _savingsService.GetAllSAAsync();
        return Ok(result);
    }

    [HttpPost("Plan")]
    public SavingsAccountDto CreateSavingsPlan(SavingsAccountDto savingsAccountDto)
    {
        double amountPerMonth = savingsAccountDto.TargetAmount / (savingsAccountDto.TargetDate.Month - DateTime.Now.Month + 1);
        savingsAccountDto.AmountPerMonth = amountPerMonth;
        return savingsAccountDto;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateSAccount(SavingsAccountDto savingsAccount, string accountName)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new Exception("Could not get Name claim");
        var result = await _savingsService.CreateSavingsAccount(savingsAccount, username, accountName);
        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = Roles.User)]
    public async Task<ActionResult> DeleteAnAccount()
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new Exception("Could not get Name claim");
        var result = await _savingsService.RemoveSAccount(username);
        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }
        return Ok(result.Value);
    }
}
