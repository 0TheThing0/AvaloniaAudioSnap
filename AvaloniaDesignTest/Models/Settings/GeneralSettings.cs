using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ReactiveUI;

namespace AvaloniaDesignTest.Models.Settings;

[DataContract]
public class GeneralSettings : ReactiveObject, ICloneable
{
    private string _coverPath = "Cache";
    private string _historyPath = "History";
    private int _historySize = 20;
    private Dictionary<string, string> _metadataNameParity = new Dictionary<string, string>()
    {
        {"album","Album"}
    };

    [DataMember(Name = "cover-path")]
    public string CoverPath
    {
        get => _coverPath;
        set => this.RaiseAndSetIfChanged(ref _coverPath,value);
    } 

    [DataMember(Name="history-path")] 
    public string HistoryPath  {
        get => _historyPath;
        set => this.RaiseAndSetIfChanged(ref _historyPath,value);
    }  

    [DataMember(Name="history-size")] 
    public int HistorySize  {
        get => _historySize;
        set => this.RaiseAndSetIfChanged(ref _historySize,value);
    }
    
    [DataMember(Name="name-parity")]
    public Dictionary<string, string> MetadataNameParity  {
        get => _metadataNameParity;
        set => this.RaiseAndSetIfChanged(ref _metadataNameParity,value);
    } 

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