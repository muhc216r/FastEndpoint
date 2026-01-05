namespace BaseApi.Feature.Endpoint;

public class FileUpload(AppDbContext db) : Endpoint<FileUploadRequest, FileUploadResponse>
{
    public override void Configure()
    {
        Post("file/upload");
        AllowFileUploads(dontAutoBindFormData: true);
        Permissions(GetType().Name);
    }

    public override async Task HandleAsync(FileUploadRequest command, CancellationToken cancellation)
    {
        await foreach (var section in FormFileSectionsAsync(cancellation))
        {
            if (section is not null)
            {
                var storagePath = Path.Combine(Env.WebRootPath ?? Env.ContentRootPath, "Storage","File");
                Directory.CreateDirectory(storagePath);

                var extension = Path.GetExtension(section.FileName);
                var storedFile = new StoredFile(extension, section.Section.ContentType!, storagePath);
                db.Set<StoredFile>().Add(storedFile);

                await using var fileStream = File.Create(storedFile.StoragePath);
                await section.Section.Body.CopyToAsync(fileStream, 1024 * 64, cancellation);

                
                await db.SaveChangesAsync(cancellation);

                var size =fileStream.Length / 1024d / 1024d;
                Response = new FileUploadResponse("/api/file/" + storedFile.Id, $"{size:F2} MB");
            }
        }

        //reading the value of a form field
        //if (sec.IsFormSection && sec.FormSection.Name == "formFieldName")
        //{
        //    var formFieldValue = await sec.FormSection.GetValueAsync(ct);
        //}
    }
}