using System.Reflection.Emit;
using GravitySwingData.Data;
using GravitySwingData.DTOs;
using GravitySwingData.Extension;
using GravitySwingData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GravitySwingData.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/user/{guid}
    [HttpGet("{guid}")]
    public async Task<IActionResult> GetUser(string guid)
    {
        var user = await _context.Users
            .Where(u => u.Guid == guid)
            .Select(u => new
            {
                u.Guid,
                u.Username,
                u.BestScore,
                u.BestCombo,
                u.BestDistance,
                u.GamesPlayed,
                u.LastPlayed,
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound("User not found.");

        return Ok(user);
    }

    // POST: api/user/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        if (registerDTO == null)
            return BadRequest("Invalid request");

        if (string.IsNullOrWhiteSpace(registerDTO.Username))
            return BadRequest("Username is required");
        if (string.IsNullOrWhiteSpace(registerDTO.Guid))
            return BadRequest("GUID is required");
        if (string.IsNullOrWhiteSpace(registerDTO.Signature))
            return BadRequest("Signature is required");

        var expectedSignature = SignatureValidator.ComputeSignature(registerDTO.Guid, registerDTO.Username);
        if (!string.Equals(expectedSignature, registerDTO.Signature, StringComparison.OrdinalIgnoreCase))
            return BadRequest("Invalid signature");
        if (registerDTO.Username.Length > 12)
            return BadRequest("Username must be 12 characters or less");
        if (!registerDTO.Username.All(c => char.IsLetterOrDigit(c)))
            return BadRequest("Username can only contain letters and numbers");
        if ( await _context.Users.AnyAsync(u => u.Username == registerDTO.Username))
            return BadRequest("Username already taken");
        // Check if GUID already exists (device/user uniqueness)
        if (await _context.Users.AnyAsync(u => u.Guid == registerDTO.Guid))
            return Ok(registerDTO.Guid); // Idempotent response for existing user

        var user = new Users
        {
            Guid = registerDTO.Guid,
            Username = registerDTO.Username
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user.Guid);
    }

    [HttpPost("app-opened")]
    public async Task<IActionResult> AppOpened([FromBody] AppOpenedDTO request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Guid == request.Guid);


        var session = new AppSession
        {
            UserId = user != null ? user.Id : 0,
            DeviceInfo = request.DeviceInfo,
            AppVersion = request.AppVersion,
            OpenedAt = DateTime.UtcNow
        };

        _context.AppSessions.Add(session);
        await _context.SaveChangesAsync();

        return Ok(new { message = "App session recorded" });
    }

    [HttpPost("game-started")]
    public async Task<IActionResult> GameStarted([FromBody] UserGuidDTO request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Guid == request.Guid);

        if (user == null)
            return NotFound("User not found");

        var session = new GameSession
        {
            UserId = user.Id,
            StartedAt = DateTime.UtcNow,
            Completed = false
        };

        _context.GameSessions.Add(session);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Game session started" });
    }
}