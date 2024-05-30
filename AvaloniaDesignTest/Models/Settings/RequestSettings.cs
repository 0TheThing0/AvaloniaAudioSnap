using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Models.Settings;
[DataContract]
public class RequestSettings : ICloneable
{
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
    public FormatSettings ReleaseFormatSettings { get; set; } = new FormatSettings();
    
    [DataMember(Name="cover")] 
    public bool Cover { get; set; } = true;

    [DataMember(Name="cover-size")] 
    public int CoverSize { get; set; } = 500;

    [DataMember(Name="host-address")] 
    public string HostAddress { get; set; } = "musicbrainz.org";

    [DataMember(Name="host-port")] 
    public int HostPort { get; set; } = 443;

    [DataMember(Name="matching-rate")]
    public int MatchingRate { get; set; } = 70;

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