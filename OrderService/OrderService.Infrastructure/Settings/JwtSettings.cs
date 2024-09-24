namespace OrderService.Infrastructure.Settings;

public class JwtSettings
{
    public string AccessTokenSecret { get; set; }
    public string RefreshTokenSecret { get; set; }
    public double AccessTokenExpirationMinutes { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}