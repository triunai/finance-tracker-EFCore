using Microsoft.EntityFrameworkCore;
using FinanceTracker.Domain.Models;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Infrastructure.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<expense> Expenses { get; set; }
        public DbSet<ExpenseItem> ExpenseItems { get; set; }
        public DbSet<BudgetCategory> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(); // Add this line
        }
 
    }
}