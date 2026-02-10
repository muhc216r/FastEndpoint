namespace FastEndPoint.Feature.Domain;
public class StoredFile
{
    public StoredFile(string extension, string contentType, string storagePath)
    {
        Id = Guid.NewGuid();
        Extension = extension;
        Name = $"{Id}{Extension}";
        ContentType = contentType;
        StoragePath = Path.Combine(storagePath, Name);
    }

    private StoredFile() { }
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Extension { get; private set; }
    public string ContentType { get; private set; }
    public string StoragePath { get; private set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}