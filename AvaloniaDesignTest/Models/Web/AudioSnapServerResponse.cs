using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Web;

public class AudioSnapServerResponse
{
    [JsonPropertyName("properties")]
    public JsonNode ResponseMetadata { get; set; }
    
    [JsonPropertyName("external-links")]
    public List<string> ExternalLinks { get; set; }

    [JsonPropertyName("image-link")] public string ImageLink { get; set; } = "";
    
    [JsonPropertyName("missing-properties")]
    public List<string> MissingProperties { get; set; }
    
    [JsonPropertyName("invalid-properties")]
    public List<string> InvalidProperties { get; set; }

    public AudioSnapServerResponse()
    {
        
    }
}