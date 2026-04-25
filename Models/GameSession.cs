namespace GravitySwingData.Models;

public class GameSession
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public Users User { get; set; } = null!;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public bool Completed { get; set; } = false;
    public int? GameRecordId { get; set; } 
    public GameRecord? GameRecord { get; set; } = null!;
}