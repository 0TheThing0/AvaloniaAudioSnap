using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Nodes;
using AvaloniaDesignTest.Models.Settings;
using TagLib;

namespace AvaloniaDesignTest.ViewModels;

public class MetadataSettings
{
    public static readonly Dictionary<string, PropertyInfo?> TagFieldParity =
        new Dictionary<string, PropertyInfo?>()
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
    
    public static IEnumerable<MetadataUnit> GetMetadataUnits(Tag fileMetadata,JsonNode? node)
    {
        List<MetadataUnit> metadatas = new List<MetadataUnit>();

        foreach (var metadata in Settings.GlobalSettings.RequestSettings.ObservingMetadata)
        {
            //Getting property of the metadata
            PropertyInfo? property= null;
            TagFieldParity.TryGetValue(metadata,out property);
            
            if (property==null || !property.CanWrite || !property.CanRead)
                continue;
            
            //Getting name for show of the metadata
            string name = metadata;
            
            //Getting old value of tag
            string oldValue = MetadataUnit.ConvertToString(property!.GetValue(fileMetadata));
            
            //Getting new value
            string newValue = MetadataUnit.ConvertToString(node?[metadata]);
            
            //Setting new metadata
            metadatas.Add(new MetadataUnit(name,oldValue,newValue,property));
        }

        return metadatas;
    }
    
}