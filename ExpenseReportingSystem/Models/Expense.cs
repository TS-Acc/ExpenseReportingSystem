namespace ExpenseReportingSystem.Models
{
    public class Expense
    {
        // main table columns
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public int EmployeeId { get; set; }

        // virtual employee to hold the FK instance when reading an expense and a collection of expenselines related
        // to this expense
        public virtual Employee? Employees { get; set; }
        public virtual ICollection<Expenseline> Expenselines { get; set; }
    }
}
