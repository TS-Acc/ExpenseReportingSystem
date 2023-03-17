using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseReportingSystem.Models;
using System.Security.Cryptography.Xml;

namespace ExpenseReportingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ErsDbContext _context;

        public ExpensesController(ErsDbContext context)
        {
            _context = context;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
          if (_context.Expenses == null)
          {
              return NotFound();
          }
            return await _context.Expenses.ToListAsync();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
          if (_context.Expenses == null)
          {
              return NotFound();
          }
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        // ************ Handmade GetApprovedExpenses Method ************
        // GET: api/Expenses/approved
        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetApprovedExpenses() {
            if (_context.Expenses == null) {
                return NotFound();
            }
            return await _context.Expenses.Where(x=> x.Status == "APPROVED").ToListAsync();
        }

        // ************ Handmade GetExpensesInReview Method ************
        // GET: api/Expenses/review
        [HttpGet("review")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesInReview() {
            if (_context.Expenses == null) {
                return NotFound();
            }
            return await _context.Expenses.Where(x => x.Status == "REVIEW").ToListAsync();
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // ************ Handmade PayExpense Method ************
        // PUT: api/Expenses/pay/5
        [HttpPut("pay/{id}")]
        public async Task<IActionResult> PayExpense(int id)
        {
            Expense? expense = await _context.Expenses.FindAsync(id);
            if (expense is null)
            {
                return BadRequest();
            }
            Employee? employee = await _context.Employees.FindAsync(expense.EmployeeId);


            if (expense.Status == "PAID")
            {
                return BadRequest("Expense already paid.");    
            }

            employee.ExpensesPaid += expense.Total;
            employee.ExpensesDue -= expense.Total;
            
            expense.Status = "PAID";

            if (employee.ExpensesDue < 0)
            {
                employee.ExpensesDue = 0;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ************ Handmade ApproveExpense Method ************
        // PUT: api/Expenses/approve/5
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveExpense(int id, Expense expense) {
            if (id != expense.Id) {
                return BadRequest();
            }

            var employee = await _context.Employees.SingleOrDefaultAsync(x => x.Id == expense.EmployeeId);           
            employee.ExpensesDue += expense.Total;
            await _context.SaveChangesAsync();

            expense.Status = "APPROVED";
            return await PutExpense(id, expense);
        }

        // ************ Handmade ApproveExpense Method ************
        // PUT: api/Expenses/reject/5
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectExpense(int id, Expense expense) {
            if (id != expense.Id) {
                return BadRequest();
            }
            expense.Status = "REJECTED";
            return await PutExpense(id, expense);
        }


        [HttpPut("review/{id}")]
        public async Task<IActionResult> ReviewExpense(int id, Expense expense)
        {
            var employee = await _context.Employees.FindAsync(expense.EmployeeId);
            var expenseLine = await _context.Expenselines.SingleOrDefaultAsync(x => id == x.ExpenseId);
            switch (id == expense.Id)
            {
                case false:
                    return BadRequest();

                case true when expense.Total > 75:
                    expense.Status = "REVIEW";
                    _context.Entry(expense).State = EntityState.Modified;
                    break;

                case true when expense.Total <= 75 && employee != null:
                    expense.Status = "APPROVED";
                    employee.ExpensesDue += expense.Total;
                    _context.Entry(expense).State = EntityState.Modified;
                    break;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
          if (_context.Expenses == null)
          {
              return Problem("Entity set 'ErsDbContext.Expenses'  is null.");
          }
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpense", new { id = expense.Id }, expense);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            if (_context.Expenses == null)
            {
                return NotFound();
            }
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseExists(int id)
        {
            return (_context.Expenses?.Any(e => e.Id == id)).GetValueOrDefault();
        }


    }
}
