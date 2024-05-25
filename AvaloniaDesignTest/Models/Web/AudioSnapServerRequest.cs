using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Web;

public class AudioSnapServerRequest
{
    [JsonPropertyName("fingerprint")]
    public string Fingerprint { get; set; }
    
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    
    [JsonPropertyName("priorities")]
    public RequestPriorities Priorities { get; set; }
    
    public AudioSnapServerRequest()
    {
        
    }
}