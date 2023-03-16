using Microsoft.EntityFrameworkCore;

namespace ExpenseReportingSystem.Models
{
    public class ErsDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Expenseline> Expenselines { get; set; }

        public ErsDbContext(DbContextOptions<ErsDbContext> options) : base(options) { }
    }
}
