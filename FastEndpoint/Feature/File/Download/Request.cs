using Microsoft.AspNetCore.Mvc;

namespace FastEndPoint.Feature.Endpoint;
public class FileDownloadRequest
{
    [FromRoute]
    public Guid Id { get; set; }
};