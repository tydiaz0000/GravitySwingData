namespace GravitySwingData.Models;

public class Users
{
    public int Id { get; set; }
    public string Guid { get; set; } = new Guid().ToString();
    public string Username { get; set; } = string.Empty;
    public int BestScore { get; set; }
    public int BestCombo { get; set; }
    public int BestDistance { get; set; }
    public int GamesPlayed { get; set; }
    public DateTime LastPlayed { get; set; } = DateTime.UtcNow;



    public List<GameRecord> GameRecords { get; set; } = new();
    public List<AppSession> AppSessions { get; set; } = new();
    public List<GameSession> GameSessions { get; set; } = new();
    public List<UserFeedback> Feedbacks { get; set; } = new();
}
