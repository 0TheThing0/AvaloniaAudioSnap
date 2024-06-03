using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AvaloniaDesignTest.Models.Settings;
using AvaloniaDesignTest.ViewModels;

namespace AvaloniaDesignTest.Web;

public class AudioSnapSearchManager
{
    private static HttpClient? _client = new HttpClient()
    {
        Timeout = TimeSpan.FromSeconds(25)
    };

    private static WebClient? _webClient = new WebClient();

    public static WebClient? WebClient
    {
        get => _webClient;
    }
    public async static Task<(HttpStatusCode TaskStatus,AudioSnapServerResponse? Response)> SearchTrackOnFingerprint(MusicTrack track)
    {
        AudioSnapServerRequest request = new AudioSnapServerRequest(track.Fingerprint,
            (int)(track.Tagfile.Properties.Duration.TotalSeconds));
        JsonContent json = JsonContent.Create(request);
        
        var builder = new UriBuilder(Settings.GlobalSettings.RequestSettings.HostAddress);
        builder.Port = (int)Settings.GlobalSettings.RequestSettings.HostPort;
        try
        {
            using (HttpResponseMessage responseMessage = await _client.PostAsync(builder.Uri, json))
            {
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var response = await responseMessage.Content.ReadFromJsonAsync<AudioSnapServerResponse>();
                    return (responseMessage.StatusCode, response);
                }

                return (responseMessage.StatusCode, null);
            }
        }
        catch
        {
            return (0, null);
        }
    }
    
    
    
    
}