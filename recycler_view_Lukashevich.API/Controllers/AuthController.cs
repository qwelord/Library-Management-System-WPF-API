using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recycler_view_Lukashevich.Data;
using recycler_view_Lukashevich.DTOs;
using recycler_view_Lukashevich.Models;
using recycler_view_Lukashevich.Services;

namespace recycler_view_Lukashevich.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly LibraryDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(LibraryDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || user.PasswordHash != request.Password)
            return Unauthorized(new { message = "Неверный email или пароль" });

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role);
        return Ok(new { token, user.Id, user.Email, user.FullName, user.Role });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            return BadRequest(new { message = "Email уже используется" });

        user.RegistrationDate = DateTime.UtcNow;
        user.Role = "User";
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role);
        return Ok(new { token, user.Id, user.Email, user.FullName, user.Role });
    }
}