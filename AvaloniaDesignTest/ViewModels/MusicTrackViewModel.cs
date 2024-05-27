using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using DynamicData;
using ReactiveUI;
using TagLib;
using File = System.IO.File;

namespace AvaloniaDesignTest.ViewModels;



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
    
    private async Task LoadCoverFromURI(string coverUrl)
    {
        IsLoadingCover = true;
        Cover = null;
        //TODO: change sava method
        Uri uri = new Uri(
            coverUrl);
        _coverPath = uri.Segments.Last();
        if (!File.Exists($"./Cache/{_coverPath}"))
        {
            if (!Directory.Exists("./Cache"))
            {
                Directory.CreateDirectory("./Cache");
            }
            using (WebClient client = new WebClient())
            {
                await client.DownloadFileTaskAsync(uri, $"./Cache/{_coverPath}").ConfigureAwait(false);
            }
        }

        //Should change file
        await LoadCoverFromFile();
        IsLoadingCover = false;
    }
    
    public async Task GetMetadata(JsonNode? node = null)
    {
        MetadataUnits.Clear();
        MetadataUnits.AddRange(MetadataSettings.GlobalSettings.GetMetadataUnits(_file.Tag, node));
        Artist = MetadataUnits.FirstOrDefault(x=>x.FieldName == "artists",null);
        Title = MetadataUnits.FirstOrDefault(x=>x.FieldName == "title",null);
    }

    public async Task SearchFileOnline()
    {
        //TODO: Server request
        var jsonstring = File.ReadAllText("testjson.json");
        JsonNode node = JsonNode.Parse(jsonstring);

        await GetMetadata(node);
        await LoadCoverFromURI("https://ia800201.us.archive.org/16/items/mbid-0875029f-362b-437a-aa84-0f9d6601730e/mbid-0875029f-362b-437a-aa84-0f9d6601730e-38463815033.png");
    }

    public async Task SetLocalImage()
    {
        IsLoadingCover = true;
        var image = _file.Tag.Pictures.FirstOrDefault(x => x.Type == PictureType.FrontCover, null);
        if (image is not null)
        {
            using (var stream = new MemoryStream(image.Data.Data))
            {
                Cover = await Task.Run(() => Bitmap.DecodeToWidth(stream, 400));
            }
        }
        else
        {
            Cover = null;
        }
        IsLoadingCover = false;
    }
    
    public async Task Analyze()
    {
        await GetMetadata();
        await SetLocalImage();
    }

    public async Task ConfigureData()
    {
        _file = TagLib.File.Create(_filepath);
        await UpdateOldValues();
        Artist = MetadataUnits.FirstOrDefault(x=>x.FieldName == "artists",null);
        Title = MetadataUnits.FirstOrDefault(x=>x.FieldName == "title",null);
    }
    
    public async Task ApplyChanges()
    {
        foreach (var unit in MetadataUnits)
        {
            unit.ApplyChange(_file.Tag);
        }

        if (_coverPath != "" && !IsLoadingCover && File.Exists($"./Cache/{_coverPath}"))
        {
            //TODO: redo
            Picture pic = new Picture()
            {
                Type = PictureType.FrontCover,
                MimeType = System.Net.Mime.MediaTypeNames.Image.Bmp
            };
            pic.Data = TagLib.ByteVector.FromPath($"./Cache/{_coverPath}");
            _file.Tag.Pictures = new TagLib.IPicture[] { pic };
        }

        _coverPath = "";
        _file.Save();
        await UpdateOldValues();
        Artist = MetadataUnits.FirstOrDefault(x=>x.FieldName == "artists",null);
        Title = MetadataUnits.FirstOrDefault(x=>x.FieldName == "title",null);
        SetLocalImage().RunSynchronously();
    }

    private async Task UpdateOldValues()
    {
        foreach (var unit in MetadataUnits)
        {
            unit.UpdatePropertyInfo();
            unit.OldValue = MetadataUnit.ConvertToString(unit.Property?.GetValue(_file.Tag));
        }
    }
    
    public async Task LoadCoverFromFile()
    {
        IsLoadingCover = true;
        //Change
        if (File.Exists($"./Cache/{this._coverPath}"))
        {
            var imageStream = await File.ReadAllBytesAsync($"./Cache/{this._coverPath}");
            using (MemoryStream a = new MemoryStream(imageStream))
            {
                Cover = await Task.Run(() => Bitmap.DecodeToWidth(a, 400));

            }
        }
        else
        {
            await SetLocalImage();
        }
        IsLoadingCover = false;
    }

    public async Task SaveAsync()
    {
        if (!Directory.Exists("./History"))
        {
            Directory.CreateDirectory("./History");
        }

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

        using (var fs = File.Create($"./History/{filename}"))
        {
            await SaveToStreamAsync(this, fs);
        }
    }
    private static async Task SaveToStreamAsync(MusicTrackViewModel data, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, data);
    }
    
    public static async Task<MusicTrackViewModel> LoadFromStream(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<MusicTrackViewModel>(stream).ConfigureAwait(false))!;
    }

    public static async Task<IEnumerable<MusicTrackViewModel>> LoadCachedAsync()
    {
        if (!Directory.Exists("./History"))
        {
            Directory.CreateDirectory("./History");
        }

        var results = new List<MusicTrackViewModel>();

        foreach (var file in Directory.EnumerateFiles("./History"))
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