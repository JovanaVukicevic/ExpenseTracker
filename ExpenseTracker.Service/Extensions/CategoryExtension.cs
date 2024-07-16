using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Extensions;

public static class CategoryExtension
{
    public static Category ToCategory(this CategoryDto categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            BudgetCap = categoryDto.BudgetCap,
            Indicator = categoryDto.Indicator
        };
        return category;
    }

    public static CategoryDto ToDto(this Category category)
    {
        var categoryDto = new CategoryDto
        {
            Name = category.Name,
            BudgetCap = category.BudgetCap,
            Indicator = category.Indicator
        };
        return categoryDto;
    }
}
