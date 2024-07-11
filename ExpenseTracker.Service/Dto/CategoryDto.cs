using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Service.Dto;

public class CategoryDto
{
    public string Name { get; set; }
    public double BudgetCap { get; set; }
    public char Indicator { get; set; }

}
