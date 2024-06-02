using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using AvaloniaDesignTest.Models.Settings;

namespace AvaloniaDesignTest.Web;

public class RequestPriorities
{
    [JsonPropertyName("release-format")]
    public FormatSettingsRequest ReleaseFormat { get; set; }
    
    [JsonPropertyName("release-country")]
    public List<string> ReleaseCountry { get; set; }
    public RequestPriorities(FormatSettings formatSettings, List<string> releaseCountry)
    {
        ReleaseCountry = releaseCountry;
        ReleaseFormat = new FormatSettingsRequest(formatSettings);
    }
}