namespace GravitySwingData.DTOs;

public class SubmitGameRecordDto
{
    public string Guid { get; set; } = string.Empty;

    public int Score { get; set; }
    public int LongestCombo { get; set; }
    public int DistanceReached { get; set; }

    public int DurationSeconds { get; set; }

    public string Signature { get; set; } = string.Empty;
}