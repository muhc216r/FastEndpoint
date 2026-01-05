namespace BaseApi.Feature.Endpoint;
public class FileDownload(AppDbContext db) : Endpoint<FileDownloadRequest>
{
    public override void Configure()
    {
        Get("file/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FileDownloadRequest query,CancellationToken cancellation)
    {
        var storedFile = await db.Set<StoredFile>().FindAsync(query.Id, cancellation);
        if (!File.Exists(storedFile?.StoragePath))
        {
            await Send.NotFoundAsync(cancellation);
            return;
        }

        try
        {
            HttpContext.MarkResponseStart();
            HttpContext.Response.StatusCode = 200;
            HttpContext.Response.ContentType = storedFile!.ContentType;

            await using var fileStream = new FileStream(storedFile.StoragePath,
                FileMode.Open,FileAccess.Read,FileShare.Read,bufferSize: 64 * 1024,
                options: FileOptions.Asynchronous | FileOptions.SequentialScan);

            await fileStream.CopyToAsync(HttpContext.Response.Body, 64 * 1024, cancellation);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error downloading file with id {FileId}", query.Id);
            if (HttpContext.Response.HasStarted)
            {
                HttpContext.Abort();
                return;
            }

            await Send.ErrorsAsync(cancellation: cancellation);
        }
    }
}