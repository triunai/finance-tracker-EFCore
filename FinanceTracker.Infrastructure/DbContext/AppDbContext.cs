using Microsoft.EntityFrameworkCore;
using FinanceTracker.Domain.Models;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Infrastructure.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseItem> ExpenseItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Expense and ExpenseItem relationship
            modelBuilder.Entity<Expense>()
                .HasMany(e => e.ExpenseItems)
                .WithOne(ei => ei.Expense)
                .HasForeignKey(ei => ei.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ExpenseItem and Category relationship
            modelBuilder.Entity<ExpenseItem>()
                .HasOne(ei => ei.Category)
                .WithMany(c => c.ExpenseItems)
                .HasForeignKey(ei => ei.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Category hierarchy (self-referencing)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial data
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Food", Description = "Groceries" },
                new Category { Id = 2, Name = "Transport", Description = "Fuel" }
            );

            modelBuilder.Entity<Expense>().HasData(
                new Expense
                {
                    Id = 1,
                    Date = DateTime.Today,
                    Notes = "Groceries",
                    CreatedAt = DateTime.UtcNow,
                    ExpenseItems = new List<ExpenseItem>
                    {
                        new ExpenseItem
                        {
                            Id = 1,
                            CategoryId = 1,
                            PaymentMethod = PaymentMethod.Cash,
                            Description = "Vegetables",
                            Price = 10.5m,
                            Quantity = 2,
                            CreatedAt = DateTime.UtcNow
                        }
                    }
                }
            );
        }
    }
}