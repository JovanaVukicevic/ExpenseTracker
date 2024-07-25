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
        var allSavingsAccounts = await _savingsService.GetAllSAAsync();
        return Ok(allSavingsAccounts);
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
        var savingsAccountIsCreated = await _savingsService.CreateSavingsAccount(savingsAccount, username, accountName);
        if (savingsAccountIsCreated.IsFailure)
        {
            return Unauthorized(savingsAccountIsCreated.Error);
        }
        return Ok(savingsAccountIsCreated.Value);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = Roles.User)]
    public async Task<ActionResult> DeleteAnAccount()
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new Exception("Could not get Name claim");
        var savingsAccountDeleted = await _savingsService.RemoveSAccount(username);
        if (savingsAccountDeleted.IsFailure)
        {
            return Unauthorized(savingsAccountDeleted.Error);
        }
        return Ok(savingsAccountDeleted.Value);
    }
}
