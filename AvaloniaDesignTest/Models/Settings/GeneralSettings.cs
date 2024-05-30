using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Models.Settings;

public class GeneralSettings : ICloneable
{
    [JsonPropertyName("cover-path")] 
    public string CoverPath { get; set; } = "Cache";

    [JsonPropertyName("history-path")] 
    public string HistoryPath { get; set; } = "History";

    [JsonPropertyName("history-size")] 
    public int HistorySize { get; set; } = 20;
    
    [JsonPropertyName("name-parity")]
    public Dictionary<string, string> MetadataNameParity { get; set; } = new Dictionary<string, string>()
    {

    };

    public object Clone()
    {
        return new GeneralSettings()
        {
            CoverPath = this.CoverPath,
            HistorySize = this.HistorySize,
            HistoryPath = this.HistoryPath,
            MetadataNameParity = new Dictionary<string, string>(this.MetadataNameParity)
        };
    }
}