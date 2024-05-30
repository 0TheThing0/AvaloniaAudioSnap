using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AvaloniaDesignTest.Models.Settings;

[DataContract]
public class FormatSettings : ICloneable
{
    [DataMember(Order = 2,Name = "album")]
    public int Album { get; set; } = 50;
    [DataMember(Name="single")]
    public int Single { get; set; } = 50;
    [DataMember(Name="ep")]
    public int EP { get; set; } = 50;
    [DataMember(Name="other")]
    public int Other { get; set; } = 50;
    [DataMember(Name="broadcast")]
    public int Broadcast { get; set; } = 50;
    [DataMember(Name="audio-drama")]
    public int AudioDrama { get; set; } = 50;
    [DataMember(Name="audiobook")]
    public int Audiobook { get; set; } = 50;
    [DataMember(Name="compilation")]
    public int Compilation { get; set; } = 50;
    [DataMember(Name="dj-mix")]
    public int DjMix { get; set; } = 50;
    [DataMember(Name="demo")]
    public int Demo { get; set; } = 50;
    [DataMember(Name="field-recording")]
    public int FieldRecording { get; set; } = 50;
    [DataMember(Name="interview")]
    public int Interview { get; set; } = 50;
    [DataMember(Name="live")]
    public int Live { get; set; } = 50;
    [DataMember(Name="mixtape")]
    public int Mixtape { get; set; } = 50;
    [DataMember(Name="remix")]
    public int Remix { get; set; } = 50;
    [DataMember(Name="soundtrack")]
    public int Soundtrack { get; set; } = 50;
    [DataMember(Name="spokenword")]
    public int Spokenword { get; set; } = 50;

    public object Clone()
    {
        return MemberwiseClone();
    }
}