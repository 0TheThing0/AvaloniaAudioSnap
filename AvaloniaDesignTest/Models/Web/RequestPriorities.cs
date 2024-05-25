using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Web;

public class RequestPriorities
{
    [JsonPropertyName("release-format")]
    public Dictionary<string,float> ReleaseFormat { get; set; }
    
    [JsonPropertyName("release-country")]
    public string[] ReleaseCountry { get; set; }
    public RequestPriorities()
    {
        
    }
}