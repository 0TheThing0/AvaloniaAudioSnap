using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using AvaloniaDesignTest.Models.Settings;

namespace AvaloniaDesignTest.Web;

public class AudioSnapServerRequest
{
    [JsonPropertyName("fingerprint")] public string Fingerprint { get; set; } 

    [JsonPropertyName("duration")] public int Duration { get; set; } 
    
    [JsonPropertyName("matching-rate")]
    public double MatchingRate { get; set; } = (double)Settings.GlobalSettings.RequestSettings.MatchingRate;
    
    [JsonPropertyName("cover")]
    public bool Cover { get; set; } = Settings.GlobalSettings.RequestSettings.Cover;
    
    [JsonPropertyName("cover-size")]
    public int CoverSize { get; set; }  = (int)Settings.GlobalSettings.RequestSettings.CoverSize;
    
    [JsonPropertyName("external-links")]
    public bool Links { get; set; } = Settings.GlobalSettings.RequestSettings.ExternalLinks;
    
    [JsonPropertyName("priorities")]
    public RequestPriorities Priorities { get; set; } = new RequestPriorities(
        Settings.GlobalSettings.RequestSettings.ReleaseFormatSettings,
        Settings.GlobalSettings.RequestSettings.CountryPriorities.ToList()
    );
    
    [JsonPropertyName("release-properties")]
    public List<string> ReleaseProperties { get; set; } = Settings.GlobalSettings.RequestSettings.ObservingMetadata.ToList();
    
    public AudioSnapServerRequest(string fingerprint, int duration)
    {
        Fingerprint = fingerprint;
        Duration = duration;
    }
}