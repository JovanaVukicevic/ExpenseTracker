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
    private readonly ITransactionService _transactionService;
    private readonly IUserService _userService;
    public TransactionController(ITransactionService transactionService, IUserService userService)
    {
        _transactionService = transactionService;
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles = Roles.User)]
    public async Task<IActionResult> GetTransactionsByFilters(
        [FromQuery] int? accountId,
        char? indicator,
        string? category,
        DateTime? from,
        DateTime? until)
    {
        var usernameClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new NotFoundException("User not found");
        var user = await _userService.GetUserByUsernameAsync(usernameClaim);
        var filteredTransactions = await _transactionService.GetTransactionsByFiltersAsync(user.Id, accountId, indicator, category, from, until);
        return Ok(filteredTransactions);
    }

    [HttpPost("Incomes")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateIncome(TransactionDto transactionDto)
    {
        var usernameClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name)
             ?? throw new NotFoundException("User not found");
        var result = await _transactionService.CreateIncomeAsync(transactionDto, usernameClaim);
        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }
        return NoContent();
    }

    [HttpPost("Expenses")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateExpense(TransactionDto transactionDto)
    {
        var usernameClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name)
             ?? throw new NotFoundException("User not found");
        var result = await _transactionService.CreateExpenseAsync(transactionDto, usernameClaim);
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
        var result = await _transactionService.DeleteTransaction(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return NoContent();
    }
}
