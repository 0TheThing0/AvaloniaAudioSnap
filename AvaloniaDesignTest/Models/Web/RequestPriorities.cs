using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Web;

public class RequestPriorities
{
    [DataMember(Name="release-format")]
    public Dictionary<string,float> ReleaseFormat { get; set; }
    
    [DataMember(Name="release-country")]
    public string[] ReleaseCountry { get; set; }
    public RequestPriorities()
    {
        
    }
}