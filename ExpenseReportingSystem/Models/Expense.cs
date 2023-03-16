using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseReportingSystem.Models
{
    public class Expense
    {
        // main table columns
        public int Id { get; set; }
        [StringLength(80)]
        public string Description { get; set; } = string.Empty;
        [StringLength(10)]
        public string Status { get; set; } = "NEW";
        [Column(TypeName = "decimal(11,2)")]
        public decimal Total { get; set; } = 0;
        public int EmployeeId { get; set; }

        // virtual employee to hold the FK instance when reading an expense and a collection of expenselines related
        // to this expense
        public virtual Employee? Employees { get; set; }
        public virtual ICollection<Expenseline> Expenselines { get; set; }
    }
}
