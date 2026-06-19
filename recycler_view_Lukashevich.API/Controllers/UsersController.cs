using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recycler_view_Lukashevich.Data;
using recycler_view_Lukashevich.DTOs;
using recycler_view_Lukashevich.Models;

namespace recycler_view_Lukashevich.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public UsersController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role,
                RegistrationDate = u.RegistrationDate
            })
            .ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { message = "Пользователь не найден" });

        var dto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            RegistrationDate = user.RegistrationDate
        };
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            return BadRequest(new { message = "Email уже существует" });

        user.RegistrationDate = DateTime.UtcNow;
        user.Role = user.Role ?? "User";
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] User user)
    {
        if (id != user.Id)
            return BadRequest(new { message = "ID не совпадают" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _context.Users.FindAsync(id);
        if (existing == null)
            return NotFound(new { message = "Пользователь не найден" });

        existing.Email = user.Email;
        existing.FullName = user.FullName;
        existing.PasswordHash = user.PasswordHash;
        existing.Role = user.Role;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { message = "Пользователь не найден" });

        if (user.Role == "Admin")
            return BadRequest(new { message = "Нельзя удалить администратора" });

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}