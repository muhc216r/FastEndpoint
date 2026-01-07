namespace FastEndPoint.Feature.Domain;

public class Configuration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(x => x.Name);
        builder.Property(x => x.Name).HasMaxLength(500);
    }
}