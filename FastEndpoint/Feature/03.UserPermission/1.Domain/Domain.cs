namespace FastEndPoint.Feature.Domain;
public class UserPermission
{
    public UserPermission(int userId,string permission)
    {
        UserId = userId;
        Permission = permission;
    }
    
    private UserPermission() { }
    public long Id { get; private set; }
    public int UserId { get; private set; }
    public string Permission { get; private set; }
}