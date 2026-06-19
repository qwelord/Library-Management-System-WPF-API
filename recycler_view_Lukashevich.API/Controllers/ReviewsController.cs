using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recycler_view_Lukashevich.Data;
using recycler_view_Lukashevich.Models;

namespace recycler_view_Lukashevich.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public ReviewsController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Book)
            .ToListAsync();
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (review == null)
            return NotFound(new { message = "Отзыв не найден" });
        return Ok(review);
    }

    [HttpGet("book/{bookId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByBook(int bookId)
    {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.BookId == bookId)
            .ToListAsync();
        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Review review)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var book = await _context.Books.FindAsync(review.BookId);
        if (book == null)
            return BadRequest(new { message = "Книга не найдена" });

        var user = await _context.Users.FindAsync(review.UserId);
        if (user == null)
            return BadRequest(new { message = "Пользователь не найден" });

        review.CreatedAt = DateTime.UtcNow;
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Review review)
    {
        if (id != review.Id)
            return BadRequest(new { message = "ID не совпадают" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _context.Reviews.FindAsync(id);
        if (existing == null)
            return NotFound(new { message = "Отзыв не найден" });

        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || (existing.UserId != int.Parse(userIdClaim) && !User.IsInRole("Admin")))
            return Forbid();

        existing.Rating = review.Rating;
        existing.Comment = review.Comment;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
            return NotFound(new { message = "Отзыв не найден" });

        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || (review.UserId != int.Parse(userIdClaim) && !User.IsInRole("Admin")))
            return Forbid();

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}