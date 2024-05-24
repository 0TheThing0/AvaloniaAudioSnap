using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using AvaloniaDesignTest.Web;
using DynamicData;

namespace AvaloniaDesignTest.ViewModels;

public class MusicTrack
{
    private static AudioSnapSearchManager _searchManager = new();
    public string Artist { get; set; }
    public string Title { get; set; }
    public string CoverUrl { get; set; }

    public ObservableCollection<MetadataUnit> MetadataUnits { get; set; } = new ObservableCollection<MetadataUnit>();
    private string _path;
    private string _fingerprint;
    public MusicTrack(string path, string fingerprint)
    {
        _path = path;
        _fingerprint = fingerprint;
    }
    public void GetMetadata()
    {
        var tfile = TagLib.File.Create(_path);
        var jsonstring = File.ReadAllText("testjson.json");
        MetadataUnits.Clear();
        MetadataUnits.AddRange(MetadataSettings.GlobalSettings.GetMetadataUnits(tfile.Tag, JsonNode.Parse(jsonstring)));
    }
}