using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using AvaloniaDesignTest.Models.Settings;

namespace AvaloniaDesignTest.Web;

public class FormatSettingsRequest
{
    [JsonPropertyName("Album")] public int Album { get; set; }

    [JsonPropertyName("Single")] public int Single { get; set; } 
    
    [JsonPropertyName("EP")]
    public int EP
    { get; set; }
    
    [JsonPropertyName("Other")]
    public int Other{ get; set; }
    
    [JsonPropertyName("Broadcast")]
    public int Broadcast
    { get; set; }
    
    [JsonPropertyName("Audio drama")]
    public int AudioDrama{ get; set; }
    
    [JsonPropertyName("Audiobook")]
    public int Audiobook{ get; set; }
    
    [JsonPropertyName("Compilation")]
    public int Compilation
    { get; set; }
    
    [JsonPropertyName("DJ-mix")]
    public int DjMix{ get; set; }
    
    [JsonPropertyName("Demo")]
    public int Demo{ get; set; }
    
    [JsonPropertyName("Field recording")]
    public int FieldRecording{ get; set; }
    
    [JsonPropertyName("Interview")]
    public int Interview{ get; set; }
    
    [JsonPropertyName("Live")]
    public int Live{ get; set; }
    
    [JsonPropertyName("Mixtape/Street")]
    public int Mixtape{ get; set; }
    
    [JsonPropertyName("Remix")]
    public int Remix{ get; set; }
    
    [JsonPropertyName("Soundtrack")]
    public int Soundtrack{ get; set; }
    
    [JsonPropertyName("Spokenword")]
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