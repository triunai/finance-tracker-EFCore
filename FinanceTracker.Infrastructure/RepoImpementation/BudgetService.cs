using FinanceTracker.Domain.Enums;
using FinanceTracker.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Infrastructure.RepoImpementation
{
    // Services/BudgetService.cs
    public class BudgetService
    {
        private readonly AppDbContext _context;

        public BudgetService(AppDbContext context)
        {
            _context = context;
        }

        public decimal CalculateSpent(int budgetId)
        {
            var budget = _context.Budgets
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == budgetId);

            if (budget == null) return 0;

            var currentDate = DateTime.UtcNow;
            var startDate = budget.Period switch
            {
                Period.Monthly => new DateTime(currentDate.Year, currentDate.Month, 1),
                Period.Weekly => currentDate.AddDays(-7),
                Period.Yearly => new DateTime(currentDate.Year, 1, 1),
                _ => currentDate
            };

            return _context.ExpenseItems
                .Where(ei => ei.CategoryId == budget.CategoryId && ei.Expense.Date >= startDate)
                .Sum(ei => ei.Price * ei.Quantity);
        }
    }
}
