using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvaloniaDesignTest.Web;
using DynamicData;
using ReactiveUI;
using TagLib;
using TagLib.NonContainer;
using TagLib.Png;
using Avalonia.Media.Imaging;
using File = System.IO.File;

namespace AvaloniaDesignTest.ViewModels;

public class MusicTrackViewModel : ViewModelBase
{
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
    }
    public MetadataUnit Title  {
        get => _title;
    }
    
    private string _fingerprint;
    private TagLib.File _file;
    private Bitmap? _cover;
    private ObservableCollection<MetadataUnit> _metadataUnits =  new ObservableCollection<MetadataUnit>();
    private MetadataUnit _artist;
    private MetadataUnit _title;
    
    public MusicTrackViewModel(string path, string fingerprint)
    {
        _fingerprint = fingerprint;
        _file = TagLib.File.Create(path);
    }
    public async Task LoadCover()
    {
        string pngFilePath = "D:/test.png";
        Uri uri = new Uri(
            "https://ia601309.us.archive.org/28/items/mbid-f268b8bc-2768-426b-901b-c7966e76de29/mbid-f268b8bc-2768-426b-901b-c7966e76de29-12750224075_thumb500.jpg");
        using (WebClient client = new WebClient())
        {
            await client.DownloadFileTaskAsync(uri,pngFilePath).ConfigureAwait(false);
        }
        var imageStream = await File.ReadAllBytesAsync(pngFilePath);
        MemoryStream a = new MemoryStream(imageStream);
        Cover = await Task.Run(() => Bitmap.DecodeToWidth(a, 500));
    }
    public async Task GetMetadata()
    {
        //TODO: Server response
        var jsonstring = File.ReadAllText("testjson.json");
        JsonNode node = JsonNode.Parse(jsonstring);
        MetadataUnits.Clear();
        MetadataUnits.AddRange(MetadataSettings.GlobalSettings.GetMetadataUnits(_file.Tag, node));
        
        _artist = MetadataUnits.First(x=>x.Name == "artists");
        _title = MetadataUnits.First(x=>x.Name == "title");
    }

    public bool ApplyChanges()
    {
        foreach (var unit in MetadataUnits)
        {
            unit.ApplyChange(_file.Tag);
        }

        _file.Save();
        UpdateOldValues();
        return true;
    }

    private void UpdateOldValues()
    {
        foreach (var unit in MetadataUnits)
        {
            unit.OldValue = MetadataUnit.ConvertToString(unit.Property.GetValue(_file.Tag));
        }
    }

    public async Task Search()
    {
        GetMetadata();
        LoadCover();
    }
}