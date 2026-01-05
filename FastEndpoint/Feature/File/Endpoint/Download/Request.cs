using Microsoft.AspNetCore.Mvc;

namespace BaseApi.Feature.Endpoint;
public class FileDownloadRequest
{
    [FromRoute]
    public Guid Id { get; set; }
};