namespace FastEndPoint.Feature.Domain;
public class Permission
{
    public Permission(string name) => Name = name;

    private Permission() { }
    public string Name { get; private set; }
}