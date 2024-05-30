using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Models.Settings;

[DataContract]
public class GeneralSettings : ICloneable
{
    [DataMember(Name="cover-path")] 
    public string CoverPath { get; set; } = "Cache";

    [DataMember(Name="history-path")] 
    public string HistoryPath { get; set; } = "History";

    [DataMember(Name="history-size")] 
    public int HistorySize { get; set; } = 20;
    
    [DataMember(Name="name-parity")]
    public Dictionary<string, string> MetadataNameParity { get; set; } = new Dictionary<string, string>()
    {
        {"album","Album"}
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