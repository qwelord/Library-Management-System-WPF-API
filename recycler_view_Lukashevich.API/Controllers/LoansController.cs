using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recycler_view_Lukashevich.Data;
using recycler_view_Lukashevich.Models;

namespace recycler_view_Lukashevich.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public LoansController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var loans = await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .ToListAsync();
        return Ok(loans);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var loan = await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.Id == id);
        if (loan == null)
            return NotFound(new { message = "Выдача не найдена" });
        return Ok(loan);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var loans = await _context.Loans
            .Include(l => l.Book)
            .Where(l => l.UserId == userId)
            .ToListAsync();
        return Ok(loans);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Loan loan)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var book = await _context.Books.FindAsync(loan.BookId);
        if (book == null)
            return BadRequest(new { message = "Книга не найдена" });

        if (book.AvailableQuantity <= 0)
            return BadRequest(new { message = "Нет доступных экземпляров книги" });

        var user = await _context.Users.FindAsync(loan.UserId);
        if (user == null)
            return BadRequest(new { message = "Пользователь не найден" });

        loan.LoanDate = DateTime.UtcNow;
        loan.DueDate = loan.DueDate == default ? DateTime.UtcNow.AddDays(14) : loan.DueDate;
        loan.Status = "Active";

        book.AvailableQuantity--;

        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    [HttpPut("{id}/return")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null)
            return NotFound(new { message = "Выдача не найдена" });

        if (loan.Status == "Returned")
            return BadRequest(new { message = "Книга уже возвращена" });

        loan.ReturnDate = DateTime.UtcNow;
        loan.Status = "Returned";

        var book = await _context.Books.FindAsync(loan.BookId);
        if (book != null)
            book.AvailableQuantity++;

        await _context.SaveChangesAsync();
        return Ok(loan);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null)
            return NotFound(new { message = "Выдача не найдена" });

        if (loan.Status == "Active")
            return BadRequest(new { message = "Нельзя удалить активную выдачу, сначала верните книгу" });

        _context.Loans.Remove(loan);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}