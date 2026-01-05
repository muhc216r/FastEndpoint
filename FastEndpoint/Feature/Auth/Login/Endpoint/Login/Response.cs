using System.Globalization;

namespace IdentityServer.Feature.Endpoint;
public class AuthLoginResponse
{
    public AuthLoginResponse(string token, string refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }

    public string Token { get; set; }
    public string RefreshToken { get; set; }


    public string TokenExpiry =>
        TimeZoneInfo.ConvertTime(DateTime.UtcNow.Add(AuthConfig.TokenLifetime), GetIranTimeZone()).ToString("O", CultureInfo.InvariantCulture);

    public string RefreshTokenExpiry
        => TimeZoneInfo.ConvertTime(DateTime.UtcNow.Add(AuthConfig.RefreshTokenLifetime), GetIranTimeZone()).ToString("O", CultureInfo.InvariantCulture);

    private static TimeZoneInfo GetIranTimeZone()
    {
        try { return TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time"); }// Windows
        catch (TimeZoneNotFoundException) { return TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran"); }// Linux
    }
}
