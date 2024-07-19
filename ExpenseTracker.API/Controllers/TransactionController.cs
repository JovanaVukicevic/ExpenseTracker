using System.Security.Claims;
using ExpenseTracker.Repository.Constants;
using ExpenseTracker.Service.CustomException;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;


[ApiController]
[Route("api/[controller]s")]
[ServiceFilter(typeof(CustomExceptionFilter))]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transService;
    private readonly IUserService _userService;

    public TransactionController(ITransactionService transactionService, IUserService userService)
    {
        _transService = transactionService;
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Roles = Roles.User)]
    public async Task<List<TransactionDto>> GetTransactionsByFilters(int? accountId, char? indicator, string? category, DateTime? from, DateTime? until)
    {
        var usernameClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var user = await _userService.GetUserByUsernameAsync(usernameClaim);
        var result = await _transService.GetTransactionsByFiltersAsync(user.Id, accountId, indicator, category, from, until);
        return result;
    }

    [HttpPost("Income")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateIncome(TransactionDto transDto)
    {
        var usernameClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var result = await _transService.CreateIncomeAsync(transDto, usernameClaim);
        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }
        return NoContent();
    }

    [HttpPost("Expense")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateExpense(TransactionDto transDto)
    {
        var usernameClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var result = await _transService.CreateExpenseAsync(transDto, usernameClaim);
        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }
        return NoContent();

    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> DeleteTransaction(int id)
    {
        var result = await _transService.DeleteTransaction(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return NoContent();

    }


}
