namespace FastEndPoint.Feature.Domain;

public class Configuration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.HasKey(x => x.Key);
    }
}