using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Models.Settings;

public class Settings : ICloneable
{
    public static Settings GlobalSettings = new Settings();

    [JsonPropertyName("general-settings")] 
    public GeneralSettings GeneralSettings { get; set; } = new GeneralSettings();

    [JsonPropertyName("request-settings")]
    public RequestSettings RequestSettings { get; set; } = new RequestSettings();
    
    public object Clone()
    {
        return new Settings()
        {
            GeneralSettings = GeneralSettings.Clone() as GeneralSettings,
            RequestSettings = RequestSettings.Clone() as RequestSettings
        };
    }
    
}