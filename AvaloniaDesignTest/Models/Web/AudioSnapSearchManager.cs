using System;
using System.Net.Http;
using AvaloniaDesignTest.ViewModels;

namespace AvaloniaDesignTest.Web;

public class AudioSnapSearchManager
{
    private static HttpClient? _client = new HttpClient();
    
    //TODO: Do realization
    public AudioSnapServerResponse SearchTrackOnFingerprint(MusicTrackViewModel track)
    {
        return null;
    }
    
    
}