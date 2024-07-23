using System.Security.Claims;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(CustomExceptionFilter))]
public class ScheduledController : ControllerBase
{
    private readonly IScheduledService _scheduledService;

    public ScheduledController(IScheduledService scheduledService)
    {
        _scheduledService = scheduledService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllScheduledTransactions()
    {
        var result = await _scheduledService.GetAllScheduledTransactionsAsync();
        return Ok(result);
    }

    [HttpPost("Income")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateScheduledIncome(ScheduledDto scheduledDto)
    {
        var usernameClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new Exception("Could not get Name claim");
        var result = await _scheduledService.CreateScheduledIncomeAsync(scheduledDto, usernameClaim);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return NoContent();
    }

    [HttpPost("Expense")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateScheduledExpense(ScheduledDto scheduledDto)
    {
        var usernameClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new Exception("Could not get Name claim");
        var result = await _scheduledService.CreateScheduledExpenseAsync(scheduledDto, usernameClaim);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> DeleteScheduledTransaction(int id)
    {
        var result = await _scheduledService.DeleteScheduledAsync(id);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return NoContent();
    }
}
