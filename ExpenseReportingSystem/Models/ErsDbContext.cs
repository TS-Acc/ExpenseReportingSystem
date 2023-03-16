using Microsoft.EntityFrameworkCore;

namespace ExpenseReportingSystem.Models
{
    public class ErsDbContext : DbContext
    {


        public ErsDbContext(DbContextOptions<ErsDbContext> options) : base(options) { }
    }
}
