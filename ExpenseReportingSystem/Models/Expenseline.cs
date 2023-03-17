using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ExpenseReportingSystem.Models {
    public class Expenseline {
        public int Id { get; set; }
        public int Quantity { get; set; } = 1;

        // FK's and Virtual Properties
        
        public int ExpenseId { get; set; }

        [JsonIgnore]
        public virtual Expense? Expense { get; set; }

        public int ItemId { get; set; }
        public virtual Item? Item { get; set; }
    }
}
