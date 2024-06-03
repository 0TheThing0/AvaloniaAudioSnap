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
        {"album","Album"},
        {"artists", "Artists"},
        {"title", "Title"},
        {"album-artists", "Album artists"},
        {"album-artists-sort", "Album artists (sorted)"},
        {"disc", "Disc"},
        {"disc-count", "Disc count"},
        {"genres", "Genres"},
        {"isrc", "ISRC"},
        {"length", "Length"},
        {"music-brainz-artist-id", "MusicBrainz artist ID"},
        {"music-brainz-release-id", "MusicBrainz release ID"},
        {"music-brainz-release-status", "MusicBrainz release status"},
        {"music-brainz-track-id","MusicBrainz track ID"},
        {"track", "Track"},
        {"track-count", "Track count"},
        {"year", "Year"}
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
    public int? HistorySize
    {
        get => _historySize;
        set
        {
            if (value is not null)
            {
                value = Math.Min((int)value, 100);
                value = Math.Max((int)value, 1);
                this.RaiseAndSetIfChanged(ref _historySize, (int)value);
            }
        }
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