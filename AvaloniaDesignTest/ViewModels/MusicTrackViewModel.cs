using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using DynamicData;
using ReactiveUI;
using TagLib;
using File = System.IO.File;

namespace AvaloniaDesignTest.ViewModels;



public class MusicTrackViewModel : ViewModelBase
{
    public bool IsLoadingCover
    {
        get => _isLoadingCover;
        set => this.RaiseAndSetIfChanged(ref _isLoadingCover,value);
    }
    public ObservableCollection<MetadataUnit> MetadataUnits { 
        get => _metadataUnits;
        set => this.RaiseAndSetIfChanged(ref _metadataUnits,value); }
    
    public Bitmap? Cover
    {
        get => _cover;
        private set => this.RaiseAndSetIfChanged(ref _cover, value);
    }

    public MetadataUnit Artist  {
        get => _artist;
        private set => this.RaiseAndSetIfChanged(ref _artist, value);
    }
    public MetadataUnit Title  {
        get => _title;
        private set => this.RaiseAndSetIfChanged(ref _title, value);
    }
    
    private string _fingerprint;
    private TagLib.File _file;
    private Bitmap? _cover;
    private ObservableCollection<MetadataUnit> _metadataUnits = new ObservableCollection<MetadataUnit>();
    private MetadataUnit _artist;
    private MetadataUnit _title;
    private bool _isLoadingCover = false;
    
    
    public MusicTrackViewModel(string path, string fingerprint)
    {
        _fingerprint = fingerprint;
        _file = TagLib.File.Create(path);
    }
    
    private async Task LoadCover(string coverUrl)
    {
        IsLoadingCover = true;
        Cover = null;
        //TODO: change sava method
        string pngFilePath = "D:/test.png";
        Uri uri = new Uri(
            coverUrl);
        using (WebClient client = new WebClient())
        {
            await client.DownloadFileTaskAsync(uri,pngFilePath).ConfigureAwait(false);
        }
        var imageStream = await File.ReadAllBytesAsync(pngFilePath);
        MemoryStream a = new MemoryStream(imageStream);
        Cover = await Task.Run(() => Bitmap.DecodeToWidth(a, 500));
        IsLoadingCover = false;
    }
    
    public async Task GetMetadata(JsonNode? node = null)
    {
        MetadataUnits.Clear();
        MetadataUnits.AddRange(MetadataSettings.GlobalSettings.GetMetadataUnits(_file.Tag, node));
        Artist = MetadataUnits.First(x=>x.Name == "artists");
        Title = MetadataUnits.First(x=>x.Name == "title");
    }

    public async Task SearchFileOnline()
    {
        //TODO: Server request
        var jsonstring = File.ReadAllText("testjson.json");
        JsonNode node = JsonNode.Parse(jsonstring);

        await GetMetadata(node);
        await LoadCover("https://ia800201.us.archive.org/16/items/mbid-0875029f-362b-437a-aa84-0f9d6601730e/mbid-0875029f-362b-437a-aa84-0f9d6601730e-38463815033.png");
    }

    public async Task SetLocalImage()
    {
        IsLoadingCover = true;
        var image = _file.Tag.Pictures.FirstOrDefault(x => x.Type == PictureType.FrontCover, null);
        if (image is not null) 
            Cover = new Bitmap(new MemoryStream(image.Data.Data));
        IsLoadingCover = false;
    }
    
    public async Task Analyze()
    {
        await GetMetadata();
        await SetLocalImage();
    }
    
    
    public bool ApplyChanges()
    {
        foreach (var unit in MetadataUnits)
        {
            unit.ApplyChange(_file.Tag);
        }

        //TODO: redo
        Picture pic = new Picture()
        {
            Type = PictureType.FrontCover,
            MimeType = System.Net.Mime.MediaTypeNames.Image.Bmp
        };
        pic.Data = TagLib.ByteVector.FromPath("D:/test.bmp");
        _file.Tag.Pictures =  new TagLib.IPicture[]{ pic };
        _file.Save();
        UpdateOldValues();
        return true;
    }

    private async void UpdateOldValues()
    {
        foreach (var unit in MetadataUnits)
        {
            unit.OldValue = MetadataUnit.ConvertToString(unit.Property.GetValue(_file.Tag));
        }

        await SetLocalImage();
    }
}