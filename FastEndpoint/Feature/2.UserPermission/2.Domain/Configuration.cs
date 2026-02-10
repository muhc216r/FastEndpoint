namespace FastEndPoint.Feature.Domain;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.Property(x => x.Permission).HasMaxLength(500);
    }
}