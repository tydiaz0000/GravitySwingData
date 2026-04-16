using GravitySwingData.Data;
using GravitySwingData.DTOs;
using GravitySwingData.Extension;
using GravitySwingData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GravitySwingData.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly AppDbContext _context;

    // Tune these based on your game mechanics
    private const int MAX_SCORE_PER_SECOND = 500;
    private const int MAX_COMBO = 1000;
    private const int MAX_DISTANCE = 999999;

    public GameController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitGameRecordDto request)
    {
        // 🔹 1. Validate user via GUID (never trust UserId from client)

        var expectedSignature = SignatureValidator.ComputeSignature(
            request.Guid,
            request.Score,
            request.LongestCombo,
            request.DistanceReached,
            request.DurationSeconds
        );

        if (!string.Equals(expectedSignature, request.Signature, StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Invalid signature");
        }



        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Guid == request.Guid);

        if (user == null)
            return NotFound("User not found");

        // 🔹 2. Basic validation
        if (request.Score < 0 ||
            request.LongestCombo < 0 ||
            request.DistanceReached < 0 ||
            request.DurationSeconds <= 0)
        {
            return BadRequest("Invalid values");
        }

        // 🔹 3. Anti-cheat: sanity limits
        if (request.LongestCombo > MAX_COMBO ||
            request.DistanceReached > MAX_DISTANCE)
        {
            return BadRequest("Values exceed limits");
        }

        // 🔹 4. Anti-cheat: score vs time check
        var maxPossibleScore = request.DurationSeconds * MAX_SCORE_PER_SECOND;

        bool isSuspicious = false;

        if (request.Score > maxPossibleScore)
        {
            isSuspicious = true;
        }

        // 🔹 5. Rate limiting (simple: prevent spam submissions)
        var lastGame = await _context.GameRecords
            .Where(g => g.UserId == user.Id)
            .OrderByDescending(g => g.PlayedAt)
            .FirstOrDefaultAsync();

        if (lastGame != null)
        {
            var secondsSinceLast = (DateTime.UtcNow - lastGame.PlayedAt).TotalSeconds;

            if (secondsSinceLast < 1) // adjust as needed
            {
                return BadRequest("Too many submissions");
            }
        }

        // 🔹 6. Save record
        var record = new GameRecord
        {
            UserId = user.Id,
            Score = request.Score,
            LongestCombo = request.LongestCombo,
            DistanceReached = request.DistanceReached,
            PlayedAt = DateTime.UtcNow
        };

        // Optional: if you added IsSuspicious column
        // record.IsSuspicious = isSuspicious;

        if (user.BestScore < request.Score)
        {
            user.BestScore = request.Score;
        }

        if (user.BestCombo < request.LongestCombo)
        {
            user.BestCombo = request.LongestCombo;
        }

        if (user.BestDistance < request.DistanceReached)
        {
            user.BestDistance = request.DistanceReached;
        }

        user.GamesPlayed += 1;
        
        _context.GameRecords.Add(record);
        await _context.SaveChangesAsync();

        // 🔹 7. Optional: update cached best score in Users table
        // (only if you add these fields)
        /*
        if (request.Score > user.BestScore)
        {
            user.BestScore = request.Score;
            await _context.SaveChangesAsync();
        }
        */

        return Ok(new
        {
            message = isSuspicious ? "Recorded (flagged as suspicious)" : "Recorded",
            record.Id,
            record.Score
        });
    }
}