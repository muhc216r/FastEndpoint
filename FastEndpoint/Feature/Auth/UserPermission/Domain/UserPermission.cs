namespace IdentityServer.Feature.Domain;
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

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.Property(x => x.Permission).HasMaxLength(500);
    }
}