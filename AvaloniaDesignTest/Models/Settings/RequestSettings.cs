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
    private bool _links = true;
    private int _coverSize = 500;
    private string _hostAddress = "musicbrainz.org";
    private int _hostPort = 443;
    private double _matchingRate = 0.5;
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
    public bool Cover
    {
        get => _cover;
        set
        {
            this.RaiseAndSetIfChanged(ref _cover, value);
        }
    }
    
    [DataMember(Name="external-links")] 
    public bool ExternalLinks
    {
        get => _links;
        set
        {
            this.RaiseAndSetIfChanged(ref _links, value);
        }
    }

    [DataMember(Name="cover-size")] 
    public int CoverSize
    {
        get => _coverSize;
        set
        {
            value = Math.Min(1200, value);
            value = Math.Max(250, value);
            this.RaiseAndSetIfChanged(ref _coverSize, value);
        }
    }

    [DataMember(Name="host-address")] 
    public string HostAddress {
        get => _hostAddress;
        set => this.RaiseAndSetIfChanged(ref _hostAddress,value);
    }  
    [DataMember(Name="host-port")] 
    public int HostPort
    {
        get => _hostPort;
        set
        {
            value = Math.Min(65535, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _hostPort, value);
        }
    }

    [DataMember(Name="matching-rate")]
    public double MatchingRate
    {
        get => _matchingRate;
        set
        {
            value = Math.Min(1, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _matchingRate, value);
        }
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