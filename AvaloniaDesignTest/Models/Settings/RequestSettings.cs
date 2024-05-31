using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ReactiveUI;

namespace AvaloniaDesignTest.Models.Settings;
[DataContract]
public class RequestSettings : ReactiveObject,ICloneable
{
    private bool _cover = true;
    private int _coverSize = 500;
    private string _hostAddress = "musicbrainz.org";
    private int _hostPort = 443;
    private int _matchingRate = 70;
    private FormatSettings _releaseFormatSettings = new FormatSettings();
    
    [DataMember(Name="observing-metadata")]
    public ObservableCollection<string> ObservingMetadata { get; set; } = new ObservableCollection<string>(){
        "album",
        "album-artists",
        "album-artists-sort",
        "artists",
        "disc",
        "disc-count",
        "first-album-artist",
        "first-album-artist-sort",
        "first-artist",
        "genres",
        "first-genre",
        "isrc",
        "length",
        "music-brainz-artist-id",
        "music-brainz-disc-id",
        "music-brainz-release-id",
        "music-brainz-release-status",
        "music-brainz-track-id",
        "track",
        "track-count",
        "title",
        "year"
    };  

    [DataMember(Name="country-priorities")]
    public ObservableCollection<string> CountryPriorities { get; set; } = new ObservableCollection<string>();

    [DataMember(Name="release-format")]
    public FormatSettings ReleaseFormatSettings {
        get => _releaseFormatSettings;
        set => this.RaiseAndSetIfChanged(ref _releaseFormatSettings,value);
    }  
    
    [DataMember(Name="cover")] 
    public bool Cover {
        get => _cover;
        set => this.RaiseAndSetIfChanged(ref _cover,value);
    }  

    [DataMember(Name="cover-size")] 
    public int CoverSize {
        get => _coverSize;
        set => this.RaiseAndSetIfChanged(ref _coverSize,value);
    }  

    [DataMember(Name="host-address")] 
    public string HostAddress {
        get => _hostAddress;
        set => this.RaiseAndSetIfChanged(ref _hostAddress,value);
    }  
    [DataMember(Name="host-port")] 
    public int HostPort {
        get => _hostPort;
        set => this.RaiseAndSetIfChanged(ref _hostPort,value);
    }  

    [DataMember(Name="matching-rate")]
    public int MatchingRate
    {
        get => _matchingRate;
        set => this.RaiseAndSetIfChanged(ref _matchingRate,value);
    }

    public RequestSettings()
    {
       
    }
    public object Clone()
    {
        return new RequestSettings()
        {
            ObservingMetadata = new ObservableCollection<string>(this.ObservingMetadata),
            CountryPriorities = new ObservableCollection<string>(this.CountryPriorities),
            ReleaseFormatSettings = this.ReleaseFormatSettings.Clone() as FormatSettings,
            Cover = this.Cover,
            CoverSize = this.CoverSize,
            HostAddress = this.HostAddress.Clone() as string,
            HostPort = this.HostPort,
            MatchingRate = this.MatchingRate
        };
    }
}