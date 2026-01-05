using System.Text;
using System.Text.Json;

namespace Common.Extension;
public static class HttpRequestMessageExtension
{
    public static HttpRequestMessage Create(HttpMethod method, string path, object? body = null)
    {
        var request = new HttpRequestMessage(method, path);
        if (body != null) request.Content = ToStringContent(body);
        return request;
    }

    private static StringContent ToStringContent(object obj)
    {
        var json = JsonSerializer.Serialize(obj, JsonConfig.Serializer);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}