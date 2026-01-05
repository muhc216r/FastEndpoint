using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Common.HttpRequest;
public class Service(ILogger<Service> logger, IHttpClientFactory clientFactory)
{
    public async Task<Response<TResult>> SendAsync<TResult>(HttpRequestMessage request, CancellationToken cancellation, [CallerMemberName] string callMember = "")
    {
        Response<TResult> response = new();

        try
        {
            var httpClient = clientFactory.CreateClient();
            logger.LogInformation($"Sending http request from {callMember} to => ({request.Method}){request.RequestUri}");

            var httpResponse = await httpClient.SendAsync(request, cancellation);
            response.StatusCode = (int)httpResponse.StatusCode;
            logger.LogInformation($"Finished sending http request from {callMember} to => {httpResponse.StatusCode}");

            response.Content = httpResponse.Content != null ? await httpResponse.Content.ReadAsStringAsync(cancellation) : null;
            logger.LogInformation($"Response: {response.Content}");

            if (httpResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                response.Result = JsonSerializer.Deserialize<TResult>(response.Content, JsonConfig.Serializer);
            }
        }
        catch (Exception exception) { response.Content += "\nException:" + exception.ToString(); }

        return response;
    }
}

public class Response<TResult>
{
    public int StatusCode { get; set; }
    public string? Content { get; set; }
    public TResult? Result { get; set; }
};

