namespace GravitySwingData.DTOs;

public class GameRecordsDTO
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public int Score { get; set; }
    public int LongestCombo { get; set; }
    public int DistanceReached { get; set; }
    public DateTime PlayedAt { get; set; }
}