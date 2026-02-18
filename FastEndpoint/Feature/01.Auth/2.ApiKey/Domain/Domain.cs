namespace FastEndPoint.Feature.Domain;
public class ApiKey
{
    public ApiKey(string appName, Guid key)
    {
        AppName = appName;
        Key = key;
    }

    private ApiKey() { }
    public Guid Key { get; private set; }
    public string AppName { get; private set; }
}