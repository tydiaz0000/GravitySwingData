namespace GravitySwingData.Models;

public class Users
{
    public int Id { get; set; }
    public string Guid { get; set; } = new Guid().ToString();
    public string Username { get; set; } = string.Empty;
    public int Score { get; set; }
    public int LongestCombo { get; set; }
    public int BestScore { get; set; }
    public int BestCombo { get; set; }
    public int BestDistance { get; set; }
    public int GamesPlayed { get; set; }
    public DateTime LastPlayed { get; set; } = DateTime.UtcNow;



    public List<GameRecord> GameRecords { get; set; } = new();
}
