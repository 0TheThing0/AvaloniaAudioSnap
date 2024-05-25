using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Nodes;
using AvaloniaDesignTest.Web;
using TagLib;

namespace AvaloniaDesignTest.ViewModels;

public class MetadataSettings
{
    public static MetadataSettings GlobalSettings = new MetadataSettings();

    public List<string> ObservingMetadata { get; set; } = new List<string>()
    {
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

    public static readonly Dictionary<string, PropertyInfo> TagFieldParity =
        new Dictionary<string, PropertyInfo>()
        {
            {"album", typeof(Tag).GetProperty("Album")},
            {"album-artists", typeof(Tag).GetProperty("AlbumArtists")},
            {"album-artists-sort", typeof(Tag).GetProperty("AlbumArtistsSort")},
            {"artists", typeof(Tag).GetProperty("Artists")},
            {"disc", typeof(Tag).GetProperty("Disc")},
            {"disc-count", typeof(Tag).GetProperty("DiscCount")},
            {"genres", typeof(Tag).GetProperty("Genres")},
            {"isrc", typeof(Tag).GetProperty("ISRC")},
            {"length", typeof(Tag).GetProperty("Length")},
            {"music-brainz-artist-id", typeof(Tag).GetProperty("MusicBrainzArtistId")},
            {"music-brainz-disc-id", typeof(Tag).GetProperty("MusicBrainzDiscId")},
            {"music-brainz-release-id", typeof(Tag).GetProperty("MusicBrainzReleaseId")},
            {"music-brainz-release-status", typeof(Tag).GetProperty("MusicBrainzReleaseStatus")},
            {"music-brainz-track-id", typeof(Tag).GetProperty("MusicBrainzTrackId")},
            {"track", typeof(Tag).GetProperty("Track")},
            {"track-count", typeof(Tag).GetProperty("TrackCount")},
            {"title", typeof(Tag).GetProperty("Title")},
            {"year", typeof(Tag).GetProperty("Year")},
        };

    public Dictionary<string, string> MetadataNameParity { get; set; } = new Dictionary<string, string>()
    {

    };
    
    public IEnumerable<MetadataUnit> GetMetadataUnits(Tag fileMetadata,JsonNode node)
    {
        List<MetadataUnit> metadatas = new List<MetadataUnit>();

        foreach (var metadata in ObservingMetadata)
        {
            //Getting property of the metadata
            PropertyInfo? property= null;
            TagFieldParity.TryGetValue(metadata,out property);
            
            if (property==null || !property.CanWrite || !property.CanRead)
                continue;
            
            //Getting name for show of the metadata
            string name = metadata;
            if (MetadataNameParity.ContainsKey(metadata))
            {
                name = MetadataNameParity[metadata];
            }
            
            //Getting old value of tag
            string oldValue = "";
            try
            {
                oldValue = MetadataUnit.ConvertToString(property!.GetValue(fileMetadata));
            }
            catch
            {

            }
            
            //Getting new value
            //TODO: Looks very bad
            string newValue = "";
            try
            {
                newValue = MetadataUnit.ConvertToString(node[metadata]);
            }
            catch {}

            //Setting new metadata
            metadatas.Add(new MetadataUnit(name,oldValue,newValue,property));
        }

        return metadatas;
    }
    
}