using GravitySwingData.Data;
using GravitySwingData.DTOs;
using GravitySwingData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GravitySwingData.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly AppDbContext _context;

    public FeedbackController(AppDbContext context)
    {
        _context = context;
    }


    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitFeedbackDTO request)
    {
        int? userId = null;

        // Resolve user if GUID is provided
        if (!string.IsNullOrWhiteSpace(request.Guid))
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Guid == request.Guid);

            if (user != null)
                userId = user.Id;
        }

        // Basic validation
        if (string.IsNullOrWhiteSpace(request.Type))
            return BadRequest("Type is required");

        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message is required");

        // Create feedback entry
        var feedback = new UserFeedback
        {
            UserId = userId,
            Type = request.Type,
            Message = request.Message,
            DeviceInfo = request.DeviceInfo,
            AppVersion = request.AppVersion,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserFeedbacks.Add(feedback);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Feedback submitted",
            feedback.Id
        });
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAll(int limit = 50)
    {
        var feedbacks = await _context.UserFeedbacks
            .OrderByDescending(f => f.CreatedAt)
            .Take(limit)
            .Select(f => new
            {
                f.Id,
                User = f.User != null ? f.User.Username : "Guest",
                f.Type,
                f.Message,
                f.DeviceInfo,
                f.AppVersion,
                f.CreatedAt,
                f.IsResolved,
                f.Priority
            })
            .ToListAsync();

        return Ok(feedbacks);
    }


    [HttpPost("resolve/{id}")]
    public async Task<IActionResult> Resolve(int id)
    {
        var feedback = await _context.UserFeedbacks
            .FirstOrDefaultAsync(f => f.Id == id);

        if (feedback == null)
            return NotFound();

        feedback.IsResolved = true;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Marked as resolved" });
    }
}