namespace FastEndPoint.Feature.Domain;
public class FileConfiguration : IEntityTypeConfiguration<StoredFile>
{
    public void Configure(EntityTypeBuilder<StoredFile> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(1000);
        builder.Property(x => x.Extension).HasMaxLength(10);
        builder.Property(x => x.ContentType).HasMaxLength(100);
        builder.Property(x => x.StoragePath).HasMaxLength(500);
    }
}