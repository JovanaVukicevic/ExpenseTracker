using System.Security.Claims;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;


[ApiController]
[Route("api/[controller]s")]
[ServiceFilter(typeof(CustomExceptionFilter))]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountService _accountService;
    public AccountController(IAccountRepository accountRepository, IAccountService accountService)
    {
        _accountRepository = accountRepository;
        _accountService = accountService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllAccounts()
    {
        var allAccounts = await _accountRepository.GetAllAccounts();
        return Ok(allAccounts);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateAnAccount(AccountDto acc)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new Exception("Could not get Name claim");
        var accountIsCreated = await _accountService.CreateAccount(acc, username);
        if (accountIsCreated.IsFailure)
        {
            return BadRequest(accountIsCreated.Error);
        }
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> DeleteAnAccount(string name)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new Exception("Could not get Name claim");
        var accountIsDeleted = await _accountService.RemoveAccount(name, username);
        if (accountIsDeleted.IsFailure)
        {
            return NotFound(accountIsDeleted.Error);
        }
        return NoContent();
    }
}