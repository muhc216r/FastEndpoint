namespace FastEndPoint.Feature.Domain;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(1000);
        builder.Property(x => x.LastName).HasMaxLength(1000);
        builder.Property(x => x.UserName).HasMaxLength(1000);
    }
}