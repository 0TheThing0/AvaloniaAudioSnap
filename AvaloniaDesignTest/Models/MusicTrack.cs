using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using AvaloniaDesignTest.Models.Settings;
using DynamicData;
using ReactiveUI;

namespace AvaloniaDesignTest.ViewModels;

[DataContract]
public class MusicTrack : ReactiveObject
{

    [DataMember(Name="filepath")]
    public string Filepath
    {
        get => _filepath;
        set => this.RaiseAndSetIfChanged(ref _filepath, value);
    }
    
    [DataMember(Name="fingerprint")]
    public string Fingerprint
    {
        get => _fingerprint;
        set => this.RaiseAndSetIfChanged(ref _fingerprint, value);
    }
    
    [DataMember(Name="metadata-units")]
    public ObservableCollection<MetadataUnit> MetadataUnits { 
        get => _metadataUnits;
        set => this.RaiseAndSetIfChanged(ref _metadataUnits,value); }
    
    [DataMember(Name="external-links")]
    public ObservableCollection<string> ExternalLinks { 
        get => _externalLinks;
        set => this.RaiseAndSetIfChanged(ref _externalLinks,value); }
    
    [IgnoreDataMember]
    public Bitmap? Cover
    {
        get => _cover;
        set => this.RaiseAndSetIfChanged(ref _cover, value);
    }

    [IgnoreDataMember]
    public MetadataUnit Artist  {
        get => _artist;
        private set => this.RaiseAndSetIfChanged(ref _artist, value);
    }
    
    [IgnoreDataMember]
    public MetadataUnit Title  {
        get => _title;
        private set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    [DataMember(Name="cover-path")]
    public string CoverPath
    {
        get => _coverPath;
        set => this.RaiseAndSetIfChanged(ref _coverPath, value);
    }

    public string LoadedFrom
    {
        get => _loadedFrom;
        set => this.RaiseAndSetIfChanged(ref _loadedFrom, value);
    }
    
    public TagLib.File Tagfile
    {
        get => _file;
        set => this.RaiseAndSetIfChanged(ref _file, value);
    }
    
    private string _fingerprint;
    private string _loadedFrom = "";
    private string _filepath;
    private string _coverPath = "";
    private TagLib.File _file;
    private Bitmap? _cover;
    private ObservableCollection<MetadataUnit> _metadataUnits = new ObservableCollection<MetadataUnit>();
    private ObservableCollection<string> _externalLinks = new ObservableCollection<string>();
    private MetadataUnit _artist;
    private MetadataUnit _title;
    
    
    public MusicTrack(string path, string fingerprint)
    {
        _fingerprint = fingerprint;
        _file = TagLib.File.Create(path);
        _filepath = path;
        _coverPath = "";
    }

    public MusicTrack() : base()
    {
        
    }
    public void GetMetadata(JsonNode? node = null)
    {
        
        MetadataUnits.Clear();
        MetadataUnits.AddRange(MetadataSettings.GetMetadataUnits(_file.Tag, node));
        Artist = MetadataUnits.FirstOrDefault(x=>x.FieldName == "artists",null);
        Title = MetadataUnits.FirstOrDefault(x=>x.FieldName == "title",null);
    }
    
    /// <summary>
    /// Update file data after loading from history
    /// </summary>
    public async Task ConfigureData()
    {
        _file = TagLib.File.Create(_filepath);
        UpdateOldValues();
        //Strange fix to fix strange bug
        ObservableCollection<MetadataUnit> newMetadataUnits = new ObservableCollection<MetadataUnit>();
        foreach (var unit in MetadataUnits)
        {
            newMetadataUnits.Add(new MetadataUnit(unit.FieldName,unit.OldValue,unit.NewValue,unit.Property));
        }

        MetadataUnits = newMetadataUnits;
        Artist = MetadataUnits.FirstOrDefault(x=>x.FieldName == "artists",null);
        Title = MetadataUnits.FirstOrDefault(x=>x.FieldName == "title",null);
    }
    
    /// <summary>
    /// Update associated properties
    /// </summary>
    public void UpdateOldValues()
    {
        foreach (var unit in MetadataUnits)
        {
            unit.UpdatePropertyInfo();
            unit.OldValue = MetadataUnit.ConvertToString(unit.Property?.GetValue(_file.Tag));
        }
    }
    
        /// <summary>
    /// Save track to history directory
    /// </summary>
    public async Task SaveAsync()
    {
        if (!Directory.Exists(Settings.GlobalSettings.GeneralSettings.HistoryPath))
        {
            Directory.CreateDirectory(Settings.GlobalSettings.GeneralSettings.HistoryPath);
        }

        //if a file was loaded from history, a new file will not be created
        //Otherwise new file will be created
        string filename;
        if (this._loadedFrom == "")
        {
                filename = Guid.NewGuid().ToString();
        }
        else
        {
            filename = _loadedFrom;
        }

        string saveStr =
            $"{Settings.GlobalSettings.GeneralSettings.HistoryPath}{Path.DirectorySeparatorChar}{filename}";
        
        using (var fs = File.Create(saveStr))
        {
            await SaveToStreamAsync(this, fs);
            _loadedFrom = saveStr;
        }
    }
    
    /// <summary>
    /// Save musictrack to stream
    /// </summary>
    /// <param name="data"></param>
    /// <param name="stream"></param>
    private static async Task SaveToStreamAsync(MusicTrack data, Stream stream)
    {
        var ser = new DataContractJsonSerializer(typeof(MusicTrack));
        ser.WriteObject(stream, data);
        
    }
    
    /// <summary>
    /// Load musictrack from stream
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static async Task<MusicTrack> LoadFromStream(Stream stream)
    {
        var ser = new DataContractJsonSerializer(typeof(MusicTrack));
        var obj =(MusicTrack)ser.ReadObject(stream);
        return obj;
    }
    
}