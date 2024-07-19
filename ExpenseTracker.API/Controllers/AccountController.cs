using System.Security.Claims;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
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
    private readonly IAuthenticationService _authService;
    private readonly IAccountService _accountService;
    public AccountController(IAccountRepository accountRepository, IAuthenticationService authService, IAccountService accountService)
    {
        _accountRepository = accountRepository;
        _authService = authService;
        _accountService = accountService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<Account>> GetAllAccounts()
    {
        var result = await _accountRepository.GetAllAccounts();
        return result;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateAnAccount(AccountDto acc)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var result = await _accountService.CreateAccount(acc, username);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> DeleteAnAccount(string name)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var result = await _accountService.RemoveAccount(name, username);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return NoContent();

    }
}