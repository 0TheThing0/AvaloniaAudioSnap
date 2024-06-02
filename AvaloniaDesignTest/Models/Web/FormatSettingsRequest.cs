using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using AvaloniaDesignTest.Models.Settings;

namespace AvaloniaDesignTest.Web;

public class FormatSettingsRequest
{
    [JsonPropertyName("album")] public int Album { get; set; }

    [JsonPropertyName("single")] public int Single { get; set; } 
    
    [JsonPropertyName("ep")]
    public int EP
    { get; set; }
    
    [JsonPropertyName("other")]
    public int Other{ get; set; }
    
    [JsonPropertyName("broadcast")]
    public int Broadcast
    { get; set; }
    
    [JsonPropertyName("audio-drama")]
    public int AudioDrama{ get; set; }
    
    [JsonPropertyName("audiobook")]
    public int Audiobook{ get; set; }
    
    [JsonPropertyName("compilation")]
    public int Compilation
    { get; set; }
    
    [JsonPropertyName("dj-mix")]
    public int DjMix{ get; set; }
    
    [JsonPropertyName("demo")]
    public int Demo{ get; set; }
    
    [JsonPropertyName("field-recording")]
    public int FieldRecording{ get; set; }
    
    [JsonPropertyName("interview")]
    public int Interview{ get; set; }
    
    [JsonPropertyName("live")]
    public int Live{ get; set; }
    
    [JsonPropertyName("mixtape")]
    public int Mixtape{ get; set; }
    
    [JsonPropertyName("remix")]
    public int Remix{ get; set; }
    
    [JsonPropertyName("soundtrack")]
    public int Soundtrack{ get; set; }
    
    [JsonPropertyName("spokenword")]
    public int Spokenword{ get; set; }

    public FormatSettingsRequest(FormatSettings settings)
    {
        Album = settings.Album;
        Single = settings.Single;
        EP = settings.EP;
        Other = settings.Other;
        Broadcast = settings.Broadcast;
        AudioDrama = settings.AudioDrama;
        Audiobook = settings.Audiobook;
        Compilation = settings.Compilation;
        DjMix = settings.DjMix;
        Demo = settings.Demo;
        FieldRecording = settings.FieldRecording;
        Interview = settings.Interview;
        Live = settings.Live;
        Mixtape = settings.Mixtape;
        Remix = settings.Remix;
        Soundtrack = settings.Soundtrack;
        Spokenword = settings.Spokenword;
    }
}