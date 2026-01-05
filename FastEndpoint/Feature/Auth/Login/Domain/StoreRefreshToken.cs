namespace IdentityServer.Feature.Domain;

public class StoreRefreshToken(int userId)
{
    public int UserId { get; } = userId;
    public string Token { get; } = Guid.NewGuid().ToString().Replace("-", "");
    public DateTime Expiry { get; } = DateTime.UtcNow.AddHours(Common.Util.Environment.IsProduction() ? 1 : 48);
}