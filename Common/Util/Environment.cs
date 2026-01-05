namespace Common.Util;
public static class Environment
{
    public static bool IsProduction()
        => GetAspNetCoreVariable("ASPNETCORE_ENVIRONMENT") == "Production";

    public static bool IsDevelopment()
        => GetAspNetCoreVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    public static string AppVersion()
        => GetAspNetCoreVariable("APP_VERSION");

    private static string GetAspNetCoreVariable(string key)
        => System.Environment.GetEnvironmentVariable(key) ?? "";
}