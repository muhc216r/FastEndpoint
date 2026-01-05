using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common;
public partial class JsonConfig
{
    public static readonly JsonSerializerOptions Serializer = new()
    {
        MaxDepth = 16,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}