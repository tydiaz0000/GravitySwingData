namespace GravitySwingData.Extension;

using System.Security.Cryptography;
using System.Text;

public static class SignatureValidator
{
    private const string SECRET = "IDMBBZzF3DU7vmAD";

    public static string ComputeSignature(string guid, int score, int combo, int distance, int duration)
    {
        var payload = $"{guid}|{score}|{combo}|{distance}|{duration}|{SECRET}";

        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(payload));

        var sb = new StringBuilder();
        foreach (var b in hash)
            sb.Append(b.ToString("x2"));

        return sb.ToString();
    }
}