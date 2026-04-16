using GravitySwingData.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GravitySwingData.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public LeaderboardController(AppDbContext context)
    {
        _context = context;
    }

    // 🔹 Get top players for all stats
    [HttpGet("top")]
    public async Task<IActionResult> GetTop(int limit = 10)
    {
        var topScore = await _context.Users
            .OrderByDescending(u => u.BestScore)
            .Take(limit)
            .Select(u => new { u.Username, u.BestScore })
            .ToListAsync();

        var topCombo = await _context.Users
            .OrderByDescending(u => u.BestCombo)
            .Take(limit)
            .Select(u => new { u.Username, u.BestCombo })
            .ToListAsync();

        var topDistance = await _context.Users
            .OrderByDescending(u => u.BestDistance)
            .Take(limit)
            .Select(u => new { u.Username, u.BestDistance })
            .ToListAsync();

        var topGames = await _context.Users
            .OrderByDescending(u => u.GamesPlayed)
            .Take(limit)
            .Select(u => new { u.Username, u.GamesPlayed })
            .ToListAsync();

        return Ok(new
        {
            score = topScore,
            combo = topCombo,
            distance = topDistance,
            gamesPlayed = topGames
        });
    }

    // 🔹 Get current player stats + ranking
    [HttpGet("me/{guid}")]
    public async Task<IActionResult> GetMyRank(string guid)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Guid == guid);

        if (user == null)
            return NotFound("User not found");

        // Rankings (1-based)
        var scoreRank = await _context.Users
            .CountAsync(u => u.BestScore > user.BestScore) + 1;

        var comboRank = await _context.Users
            .CountAsync(u => u.BestCombo > user.BestCombo) + 1;

        var distanceRank = await _context.Users
            .CountAsync(u => u.BestDistance > user.BestDistance) + 1;

        var gamesRank = await _context.Users
            .CountAsync(u => u.GamesPlayed > user.GamesPlayed) + 1;

        return Ok(new
        {
            user = new
            {
                user.Username,
                user.BestScore,
                user.BestCombo,
                user.BestDistance,
                user.GamesPlayed
            },
            ranks = new
            {
                scoreRank,
                comboRank,
                distanceRank,
                gamesRank
            }
        });
    }

    // 🔹 Optional: Get leaderboard around player (context view)
    [HttpGet("around/{guid}")]
    public async Task<IActionResult> GetAroundPlayer(string guid, int range = 5)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Guid == guid);

        if (user == null)
            return NotFound("User not found");

        var rank = await _context.Users
            .CountAsync(u => u.BestScore > user.BestScore) + 1;

        var skip = Math.Max(rank - range - 1, 0);

        var players = await _context.Users
            .OrderByDescending(u => u.BestScore)
            .Skip(skip)
            .Take(range * 2 + 1)
            .Select(u => new
            {
                u.Username,
                u.BestScore
            })
            .ToListAsync();

        return Ok(new
        {
            rank,
            players
        });
    }
}