using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExpenseTracker.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ScheduledController : ControllerBase
{
    private readonly IScheduledService _scheduledService;

    public ScheduledController(IScheduledService scheduledService)
    {
        _scheduledService = scheduledService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<List<ScheduledDto>> GetAllScheduledTransactions()
    {
        var result = await _scheduledService.GetAllScheduledTransactionsAsync();
        return result;
    }

    [HttpPost("Income")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateScheduledIncome(ScheduledDto scheduledDto)
    {
        var result = await _scheduledService.CreateScheduledIncomeAsync(scheduledDto);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return NoContent();
    }

    [HttpPost("Expense")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateScheduledExpense(ScheduledDto scheduledDto)
    {
        var result = await _scheduledService.CreateScheduledExpenseAsync(scheduledDto);
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
            return BadRequest(result.Error);
        }
        return NoContent();

    }


}