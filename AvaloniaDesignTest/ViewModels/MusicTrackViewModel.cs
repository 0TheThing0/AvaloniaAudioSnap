using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using AvaloniaDesignTest.Models.Settings;
using ReactiveUI;
using TagLib;
using File = System.IO.File;

namespace AvaloniaDesignTest.ViewModels;

//This is terrible
public class MusicTrackViewModel : ViewModelBase
{
    public bool IsLoadingCover
    {
        get => _isLoadingCover;
        set => this.RaiseAndSetIfChanged(ref _isLoadingCover, value);
    }

    public MusicTrack Track
    {
        get => _musicTrack;
        set => this.RaiseAndSetIfChanged(ref _musicTrack, value);
    }
    private bool _isLoadingCover = false;
    private MusicTrack _musicTrack;
    

    public MusicTrackViewModel(string path, string fingerprint)
    {
        _musicTrack = new MusicTrack(path, fingerprint);
    }
    
    public MusicTrackViewModel(MusicTrack track)
    {
        _musicTrack = track;
    }
    
    /// <summary>
    /// Load cover from given url, is cannot load - load from file
    /// </summary>
    /// <param name="coverUrl">Url to cover</param>
    private async Task LoadCoverFromURI(string coverUrl)
    {
        IsLoadingCover = true;
        
        Track.Cover = null;
        Uri uri = new Uri(
            coverUrl);
        Track.CoverPath = uri.Segments.Last();
        
        //Use cache file to save data
        string downloadPath =
            $"{Settings.GlobalSettings.GeneralSettings.CoverPath}{Path.DirectorySeparatorChar}{Track.CoverPath}";
        
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

    /// <summary>
    ///  Create request to server and parse answer
    /// </summary>
    /// TODO: DO normal answer to viewmodel
    public async Task SearchFileOnline()
    {
        //TODO: Server request
        var jsonstring = File.ReadAllText("testjson.json");
        
        //Parsing json response
        JsonNode metadataNode = JsonNode.Parse(jsonstring)["properties"];
        string imagePath = JsonNode.Parse(jsonstring)["image-link"].AsValue().ToString();
        
        if (imagePath != "")
        {
            await LoadCoverFromURI(
                imagePath);
        }

        Track.GetMetadata(metadataNode);

    }

    /// <summary>
    /// Set local image for pictures in track 
    /// </summary>
    public async Task SetLocalImage()
    {
        IsLoadingCover = true;
        
        var image = Track.Tagfile.Tag.Pictures.FirstOrDefault(x => x.Type == PictureType.FrontCover, null);
        if (image is not null)
        {
            using (var stream = new MemoryStream(image.Data.Data))
            {
                Track.Cover = await Task.Run(() => Bitmap.DecodeToWidth(stream, Settings.GlobalSettings.RequestSettings.CoverSize));
            }
        }
        else
        {
            Track.Cover = null;
        }
        
        IsLoadingCover = false;
    }
    
    /// <summary>
    /// Analyze file (get local metadata and local image)
    /// </summary>
    public async Task Analyze()
    {
        Track.GetMetadata();
        await SetLocalImage();
    }
    
    
    /// <summary>
    /// Apply changes for every metadata
    /// </summary>
    public async Task<bool> ApplyChanges()
    {
        bool wasError = false;
        try
        {
            foreach (var unit in Track.MetadataUnits)
            {
                unit.ApplyChange(Track.Tagfile.Tag);
            }

            //Loading cover from cache file and save it to music file
            string filepath =
                $"{Settings.GlobalSettings.GeneralSettings.CoverPath}{Path.DirectorySeparatorChar}{Track.CoverPath}";

            if (Track.CoverPath != "" && !IsLoadingCover && File.Exists(filepath))
            {
                Picture pic = new Picture()
                {
                    Type = PictureType.FrontCover,
                    MimeType = System.Net.Mime.MediaTypeNames.Image.Bmp
                };
                pic.Data = TagLib.ByteVector.FromPath(filepath);
                Track.Tagfile.Tag.Pictures = new TagLib.IPicture[] { pic };
            }

            //Saving metadata
            Track.CoverPath = "";
            Track.Tagfile.Save();
        }
        catch
        {
            wasError = true;
        }

        //Updating view
        Track.UpdateOldValues();
        await SetLocalImage();
        
        return wasError;
    }
    
    /// <summary>
    /// Load cover from path (coverpath)
    /// </summary>
    public async Task LoadCoverFromFile()
    {
        IsLoadingCover = true;
        
        string filepath =
            $"{Settings.GlobalSettings.GeneralSettings.CoverPath}{Path.DirectorySeparatorChar}{Track.CoverPath}";
        
        
        if (File.Exists(filepath))
        {
            var imageStream = await File.ReadAllBytesAsync(filepath);
            using (MemoryStream a = new MemoryStream(imageStream))
            {
                Track.Cover = await Task.Run(() => Bitmap.DecodeToWidth(a, Settings.GlobalSettings.RequestSettings.CoverSize));

            }
        }
        else
        {
            await SetLocalImage();
        }
        
        IsLoadingCover = false;
    }
    
    //Load files from history
    public static async Task<(IEnumerable<MusicTrackViewModel>,bool)> LoadCachedAsync()
    {
        bool wasError = false;
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
            try
            {
                var track = await MusicTrack.LoadFromStream(fs);
                track.LoadedFrom = Path.GetFileName(file);
                results.Add(new MusicTrackViewModel(track));
            }
            catch
            {
                wasError = true;
            }
        }
        
        return (results,wasError);
    }
}