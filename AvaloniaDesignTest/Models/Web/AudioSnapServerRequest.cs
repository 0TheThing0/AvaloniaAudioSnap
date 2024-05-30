using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Web;

public class AudioSnapServerRequest
{
    [DataMember(Name="fingerprint")]
    public string Fingerprint { get; set; }
    
    [DataMember(Name="duration")]
    public int Duration { get; set; }
    
    [DataMember(Name="priorities")]
    public RequestPriorities Priorities { get; set; }
    
    public AudioSnapServerRequest()
    {
        
    }
}