using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recycler_view_Lukashevich.Data;
using recycler_view_Lukashevich.Models;

namespace recycler_view_Lukashevich.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public BooksController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var books = await _context.Books.ToListAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return NotFound(new { message = "Книга не найдена" });
        return Ok(book);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] Book book)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _context.Books.AnyAsync(b => b.ISBN == book.ISBN))
            return BadRequest(new { message = "Книга с таким ISBN уже существует" });

        book.AvailableQuantity = book.Quantity;
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] Book book)
    {
        if (id != book.Id)
            return BadRequest(new { message = "ID в URL и теле не совпадают" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _context.Books.FindAsync(id);
        if (existing == null)
            return NotFound(new { message = "Книга не найдена" });

        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.ISBN = book.ISBN;
        existing.Year = book.Year;
        existing.Publisher = book.Publisher;
        existing.Quantity = book.Quantity;
        existing.AvailableQuantity = book.AvailableQuantity;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return NotFound(new { message = "Книга не найдена" });

        bool hasActiveLoans = await _context.Loans
            .AnyAsync(l => l.BookId == id && l.Status == "Active");
        if (hasActiveLoans)
            return BadRequest(new { message = "Нельзя удалить книгу, которая выдана читателю" });

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}