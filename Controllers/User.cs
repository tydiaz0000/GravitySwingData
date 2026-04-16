using GravitySwingData.Data;
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
                u.Id,
                u.Guid,
                u.Username
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // POST: api/user/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Users request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            return BadRequest("Username is required");

        // Check if GUID already exists (device/user uniqueness)
        var existing = await _context.Users
            .FirstOrDefaultAsync(u => u.Guid == request.Guid);

        if (existing != null)
            return Ok(existing); // return existing instead of creating duplicate

        var user = new Users
        {
            Guid = request.Guid,
            Username = request.Username
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }

    // PUT: api/user/{guid}
    [HttpPut("{guid}")]
    public async Task<IActionResult> UpdateUsername(string guid, [FromBody] string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Guid == guid);

        if (user == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(username))
            return BadRequest("Invalid username");

        user.Username = username;

        await _context.SaveChangesAsync();

        return Ok(user);
    }
}