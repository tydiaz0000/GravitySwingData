namespace GravitySwingData.Models;

public class UserFeedback
{
    public int Id { get; set; }

    public int? UserId { get; set; }   // nullable = guest allowed
    public Users? User { get; set; }

    public string Type { get; set; } = string.Empty;
    // e.g. "Bug", "Feedback", "Suggestion"

    public string Message { get; set; } = string.Empty;

    public string? DeviceInfo { get; set; }

    public string? AppVersion { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsResolved { get; set; } = false;

    public int Priority { get; set; } = 0;
    // 0 = normal, 1 = important, 2 = critical
}