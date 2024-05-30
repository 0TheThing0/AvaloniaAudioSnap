using System;
using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Models.Settings;

public class FormatSettings : ICloneable
{
    [JsonPropertyName("album")]
    public int Album { get; set; } = 50;
    [JsonPropertyName("single")]
    public int Single { get; set; } = 50;
    [JsonPropertyName("ep")]
    public int EP { get; set; } = 50;
    [JsonPropertyName("other")]
    public int Other { get; set; } = 50;
    [JsonPropertyName("broadcast")]
    public int Broadcast { get; set; } = 50;
    [JsonPropertyName("audio-drama")]
    public int AudioDrama { get; set; } = 50;
    [JsonPropertyName("audiobook")]
    public int Audiobook { get; set; } = 50;
    [JsonPropertyName("compilation")]
    public int Compilation { get; set; } = 50;
    [JsonPropertyName("dj-mix")]
    public int DjMix { get; set; } = 50;
    [JsonPropertyName("demo")]
    public int Demo { get; set; } = 50;
    [JsonPropertyName("field-recording")]
    public int FieldRecording { get; set; } = 50;
    [JsonPropertyName("interview")]
    public int Interview { get; set; } = 50;
    [JsonPropertyName("live")]
    public int Live { get; set; } = 50;
    [JsonPropertyName("mixtape")]
    public int Mixtape { get; set; } = 50;
    [JsonPropertyName("remix")]
    public int Remix { get; set; } = 50;
    [JsonPropertyName("soundtrack")]
    public int Soundtrack { get; set; } = 50;
    [JsonPropertyName("spokenword")]
    public int Spokenword { get; set; } = 50;

    public object Clone()
    {
        return MemberwiseClone();
    }
}