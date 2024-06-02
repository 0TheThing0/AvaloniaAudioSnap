using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AvaloniaDesignTest.Models.Settings;
using AvaloniaDesignTest.ViewModels;

namespace AvaloniaDesignTest.Web;

public class AudioSnapSearchManager
{
    private static HttpClient? _client = new HttpClient();
    
    //TODO: Do realization
    public async static Task<AudioSnapServerResponse> SearchTrackOnFingerprint(MusicTrack track)
    {
        AudioSnapServerRequest request = new AudioSnapServerRequest(track.Fingerprint,
            (int)(track.Tagfile.Properties.Duration.TotalSeconds));
        JsonContent json = JsonContent.Create(request);
        
        var builder = new UriBuilder(Settings.GlobalSettings.RequestSettings.HostAddress);
        builder.Port = Settings.GlobalSettings.RequestSettings.HostPort;
        var str  = JsonSerializer.Serialize(request);
        using (HttpResponseMessage responseMessage = await _client.PostAsync(builder.Uri, json))
        {
            var response = await responseMessage.Content.ReadFromJsonAsync<AudioSnapServerResponse>();
            return response;
        }
    }
    
    
}