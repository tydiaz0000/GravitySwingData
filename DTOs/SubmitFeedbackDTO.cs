namespace GravitySwingData.DTOs;

public class SubmitFeedbackDTO
{
    public string? Guid { get; set; } // optional

    public string Type { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string? DeviceInfo { get; set; }

    public string? AppVersion { get; set; }
}