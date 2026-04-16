using GravitySwingData.Data;
using GravitySwingData.DTOs;
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
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        if (registerDTO == null)
            return BadRequest("Invalid request");

        if (string.IsNullOrWhiteSpace(registerDTO.Username))
            return BadRequest("Username is required");

        // Check if GUID already exists (device/user uniqueness)


        var user = new Users
        {
            Guid = Guid.NewGuid().ToString(),
            Username = registerDTO.Username
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }


}