using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExpenseTracker.API.Controllers;


[ApiController]
[Route("api/[controller]s")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transService;

    public TransactionController(ITransactionService transactionService)
    {
        _transService = transactionService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<List<TransactionDto>> GetAllTransactions()
    {
        var result = await _transService.GetAllTransactionsAsync();
        return result;
    }

    [HttpPost("Income")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<ActionResult> CreateIncome(TransactionDto transDto)
    {
        var result = await _transService.CreateIncomeAsync(transDto);
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
    public async Task<ActionResult> CreateExpense(TransactionDto transDto)
    {
        var result = await _transService.CreateExpenseAsync(transDto);
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