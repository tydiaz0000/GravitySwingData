namespace GravitySwingData.Models;

public class GameRecord
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public Users User { get; set; } = null!;

    public int Score { get; set; }
    public int LongestCombo { get; set; }
    public int DistanceReached { get; set; }

    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
}