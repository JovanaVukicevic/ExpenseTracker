using System.Security.Claims;
using ExpenseTracker.Repository.Constants;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly ReportingService _reportingService;
    private readonly EmailService _emailService;
    private readonly IUserService _userService;

    public ReportController(ReportingService reportingService, EmailService emailService, IUserService userService)
    {
        _reportingService = reportingService;
        _emailService = emailService;
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Roles = Roles.User)]
    public async Task<ActionResult> CreateReport(int accountId)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new Exception("Could not get Name claim");
        var user = await _userService.GetUserByUsernameAsync(username);
        var file = await _reportingService.GeneratePdfAsync(accountId);
        if (user.Email is not null)
            await _emailService.SendEmailAsync(user.Email, "Monthly Report", "Your monthly report", file, "Report.pdf");
        return File(file, "application/pdf", "table.pdf");
    }
}
