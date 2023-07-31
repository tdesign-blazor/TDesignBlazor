using System.Text.Json.Serialization;

namespace TDesign;
public class UploadHttpResponse
{
    public UploadHttpResponse(int statusCode, string? statusText, string? responseText)
    {
        StatusCode = statusCode;
        StatusText = statusText;
        ResponseText = responseText;
    }

    [JsonPropertyName("status")]
    public int StatusCode { get; }

    [JsonPropertyName("statusText")]
    public string? StatusText { get; }

    [JsonPropertyName("responseText")]
    public string? ResponseText { get; }
}
