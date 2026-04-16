namespace GravitySwingData.Models;

public class AppSession
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public Users User { get; set; } = null!;

    public DateTime OpenedAt { get; set; } = DateTime.UtcNow;

    public string DeviceInfo { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
}