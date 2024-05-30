using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using AvaloniaDesignTest.Models.Settings;
using DynamicData;
using ReactiveUI;
using TagLib;
using File = System.IO.File;

namespace AvaloniaDesignTest.ViewModels;

//This is terrible

public class MusicTrackViewModel : ViewModelBase
{
    [JsonIgnore]
    public bool IsLoadingCover
    {
        get => _isLoadingCover;
        set => this.RaiseAndSetIfChanged(ref _isLoadingCover, value);
    }

    [JsonPropertyName("filepath")]
    public string Filepath
    {
        get => _filepath;
        set => this.RaiseAndSetIfChanged(ref _filepath, value);
    }
    
    [JsonPropertyName("fingerprint")]
    public string Fingerprint
    {
        get => _fingerprint;
        set => this.RaiseAndSetIfChanged(ref _fingerprint, value);
    }
    
    [JsonPropertyName("metadata-units")]
    public ObservableCollection<MetadataUnit> MetadataUnits { 
        get => _metadataUnits;
        set => this.RaiseAndSetIfChanged(ref _metadataUnits,value); }
    
    [JsonIgnore]
    public Bitmap? Cover
    {
        get => _cover;
        private set => this.RaiseAndSetIfChanged(ref _cover, value);
    }

    [JsonIgnore]
    public MetadataUnit Artist  {
        get => _artist;
        private set => this.RaiseAndSetIfChanged(ref _artist, value);
    }
    
    [JsonIgnore]
    public MetadataUnit Title  {
        get => _title;
        private set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    [JsonPropertyName("cover-path")]
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
    
    private string _fingerprint;
    private string _loadedFrom = "";
    private string _filepath;
    private string _coverPath = "";
    private TagLib.File _file;
    private Bitmap? _cover;
    private ObservableCollection<MetadataUnit> _metadataUnits = new ObservableCollection<MetadataUnit>();
    private MetadataUnit _artist;
    private MetadataUnit _title;
    private bool _isLoadingCover = false;
    
    public MusicTrackViewModel()
    {
    }
    public MusicTrackViewModel(string path, string fingerprint)
    {
        _fingerprint = fingerprint;
        _file = TagLib.File.Create(path);
        _filepath = path;
        _coverPath = "";
    }
    
    /// <summary>
    /// Load cover from given url, is cannot load - load from file
    /// </summary>
    /// <param name="coverUrl">Url to cover</param>
    private async Task LoadCoverFromURI(string coverUrl)
    {
        IsLoadingCover = true;
        
        
        Cover = null;
        Uri uri = new Uri(
            coverUrl);
        _coverPath = uri.Segments.Last();
        
        //Use cache file to save data
        string downloadPath =
            $"{Settings.GlobalSettings.GeneralSettings.CoverPath}{Path.DirectorySeparatorChar}{_coverPath}";
        
        //if this file already exists - load from file
        if (!File.Exists(downloadPath))
        {
            if (!Directory.Exists(Settings.GlobalSettings.GeneralSettings.CoverPath))
            {
                Directory.CreateDirectory(Settings.GlobalSettings.GeneralSettings.CoverPath);
            }
            
            //Load file from uri
            //TODO: Maybe redo with httpclient
            using (WebClient client = new WebClient())
            {
                await client.DownloadFileTaskAsync(uri, downloadPath).ConfigureAwait(false);
            }
        }

        //Load cover from downloaded file
        await LoadCoverFromFile();
        
        IsLoadingCover = false;
    }
    
    /// <summary>
    /// Parse data from json into metadata
    /// </summary>
    /// <param name="node">Json node</param>
    public void GetMetadata(JsonNode? node = null)
    {
        MetadataUnits.Clear();
        MetadataUnits.AddRange(MetadataSettings.GetMetadataUnits(_file.Tag, node));
        Artist = MetadataUnits.FirstOrDefault(x=>x.FieldName == "artists",null);
        Title = MetadataUnits.FirstOrDefault(x=>x.FieldName == "title",null);
    }

    /// <summary>
    ///  Create request to server and parse answer
    /// </summary>
    public async Task SearchFileOnline()
    {
        //TODO: Server request
        var jsonstring = File.ReadAllText("testjson.json");
        JsonNode node = JsonNode.Parse(jsonstring);
        
        await LoadCoverFromURI("https://ia800201.us.archive.org/16/items/mbid-0875029f-362b-437a-aa84-0f9d6601730e/mbid-0875029f-362b-437a-aa84-0f9d6601730e-38463815033.png");
        GetMetadata(node);
    }

    /// <summary>
    /// Set local image for pictures in track 
    /// </summary>
    public async Task SetLocalImage()
    {
        IsLoadingCover = true;
        
        var image = _file.Tag.Pictures.FirstOrDefault(x => x.Type == PictureType.FrontCover, null);
        if (image is not null)
        {
            using (var stream = new MemoryStream(image.Data.Data))
            {
                Cover = await Task.Run(() => Bitmap.DecodeToWidth(stream, Settings.GlobalSettings.RequestSettings.CoverSize));
            }
        }
        else
        {
            Cover = null;
        }
        
        IsLoadingCover = false;
    }
    
    /// <summary>
    /// Analyze file (get local metadata and local image)
    /// </summary>
    public async Task Analyze()
    {
        GetMetadata();
        await SetLocalImage();
    }

    /// <summary>
    /// Update file data after loading from history
    /// </summary>
    public async Task ConfigureData()
    {
        _file = TagLib.File.Create(_filepath);
        UpdateOldValues();
        Artist = MetadataUnits.FirstOrDefault(x=>x.FieldName == "artists",null);
        Title = MetadataUnits.FirstOrDefault(x=>x.FieldName == "title",null);
    }
    
    /// <summary>
    /// Apply changes for every metadata
    /// </summary>
    public async Task ApplyChanges()
    {
        foreach (var unit in MetadataUnits)
        {
            unit.ApplyChange(_file.Tag);
        }

        //Loading cover from cache file and save it to music file
        string filepath =
            $"{Settings.GlobalSettings.GeneralSettings.CoverPath}{Path.DirectorySeparatorChar}{_coverPath}";
        
        if (_coverPath != "" && !IsLoadingCover && File.Exists(filepath))
        {
            Picture pic = new Picture()
            {
                Type = PictureType.FrontCover,
                MimeType = System.Net.Mime.MediaTypeNames.Image.Bmp
            };
            pic.Data = TagLib.ByteVector.FromPath(filepath);
            _file.Tag.Pictures = new TagLib.IPicture[] { pic };
        }

        //Saving metadata
        _coverPath = "";
        _file.Save();
        
        //Updating view
        UpdateOldValues();
        SetLocalImage().RunSynchronously();
    }

    /// <summary>
    /// Update associated properties
    /// </summary>
    private void UpdateOldValues()
    {
        foreach (var unit in MetadataUnits)
        {
            unit.UpdatePropertyInfo();
            unit.OldValue = MetadataUnit.ConvertToString(unit.Property?.GetValue(_file.Tag));
        }
    }
    
    /// <summary>
    /// Load cover from path (coverpath)
    /// </summary>
    public async Task LoadCoverFromFile()
    {
        IsLoadingCover = true;
        
        
        string filepath =
            $"{Settings.GlobalSettings.GeneralSettings.CoverPath}{Path.DirectorySeparatorChar}{_coverPath}";
        
        
        if (File.Exists(filepath))
        {
            var imageStream = await File.ReadAllBytesAsync(filepath);
            using (MemoryStream a = new MemoryStream(imageStream))
            {
                Cover = await Task.Run(() => Bitmap.DecodeToWidth(a, Settings.GlobalSettings.RequestSettings.CoverSize));

            }
        }
        else
        {
            await SetLocalImage();
        }
        
        IsLoadingCover = false;
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
            using (SHA256 mySha256 = SHA256.Create())
            {
                DateTime time = DateTime.Now;
                filename = Convert.ToBase64String(mySha256.ComputeHash(BitConverter.GetBytes(time.ToBinary())));
            }
        }
        else
        {
            filename = _loadedFrom;
        }

        
        using (var fs = File.Create($"{Settings.GlobalSettings.GeneralSettings.HistoryPath}{Path.DirectorySeparatorChar}{filename}"))
        {
            await SaveToStreamAsync(this, fs);
        }
    }
    
    /// <summary>
    /// Save musictrack to stream
    /// </summary>
    /// <param name="data"></param>
    /// <param name="stream"></param>
    private static async Task SaveToStreamAsync(MusicTrackViewModel data, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, data);
    }
    
    /// <summary>
    /// Load musictrack from stream
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static async Task<MusicTrackViewModel> LoadFromStream(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<MusicTrackViewModel>(stream).ConfigureAwait(false))!;
    }

    
    //Load files from history
    public static async Task<IEnumerable<MusicTrackViewModel>> LoadCachedAsync()
    {
        if (!Directory.Exists(Settings.GlobalSettings.GeneralSettings.HistoryPath))
        {
            Directory.CreateDirectory(Settings.GlobalSettings.GeneralSettings.HistoryPath);
        }

        var results = new List<MusicTrackViewModel>();

        
        //TODO: add sort and count
        foreach (var file in Directory.EnumerateFiles(Settings.GlobalSettings.GeneralSettings.HistoryPath))
        {
            if (!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension)) continue;
            await using var fs = File.OpenRead(file);
            var track = await LoadFromStream(fs).ConfigureAwait(false);
            track.LoadedFrom = Path.GetFileName(file);
            results.Add(track);
        }

        return results;
    }
}