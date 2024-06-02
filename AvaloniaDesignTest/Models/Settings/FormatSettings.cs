using System;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ReactiveUI;

namespace AvaloniaDesignTest.Models.Settings;

[DataContract]
public class FormatSettings : ReactiveObject, ICloneable
{
    private int _album = 50;
    private int _single = 50;
    private int _ep = 50;
    private int _other = 50;
    private int _broadcast = 50;
    private int _audiobook = 50;
    private int _djMix = 50;
    private int _audioDrama = 50;
    private int _compilation = 50;
    private int _spokenword = 50;
    private int _demo = 50;
    private int _fieldRecording = 50;
    private int _interview = 50;
    private int _live = 50;
    private int _mixtape = 50;
    private int _remix = 50;
    private int _soundtrack = 50;

    [DataMember(Name = "album")]
    [JsonPropertyName("album")]
    public int Album
    {
        get => _album;
        set {   value = Math.Min(100, value);
                value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _album, value); }
    }

    [DataMember(Name = "single")]
    [JsonPropertyName("single")]
    public int Single
    {
        get => _single;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _single, value);
        }
    }

    [DataMember(Name = "ep")]
    [JsonPropertyName("ep")]
    public int EP
    {
        get => _ep;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _ep, value);
        }
    }

    [DataMember(Name = "other")]
    [JsonPropertyName("other")]
    public int Other
    {
        get => _other;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _other, value);
        }
    }

    [DataMember(Name = "broadcast")]
    [JsonPropertyName("broadcast")]
    public int Broadcast
    {
        get => _broadcast;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _broadcast, value);
        }
    }

    [DataMember(Name = "audio-drama")]
    [JsonPropertyName("audio-drama")]
    public int AudioDrama
    {
        get => _audioDrama;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _audioDrama, value);
        }
    }

    [DataMember(Name = "audiobook")]
    [JsonPropertyName("audiobook")]
    public int Audiobook
    {
        get => _audiobook;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _audiobook, value);
        }
    }

    [DataMember(Name = "compilation")]
    [JsonPropertyName("compilation")]
    public int Compilation
    {
        get => _compilation;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _compilation, value);
        }
    }

    [DataMember(Name="dj-mix")]
    [JsonPropertyName("dj-mix")]
    public int DjMix
    {
        get => _djMix;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _djMix, value);
        }
    }

    [DataMember(Name = "demo")]
    [JsonPropertyName("demo")]
    public int Demo
    {
        get => _demo;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _demo, value);
        }
    }

    [DataMember(Name = "field-recording")]
    [JsonPropertyName("field-recording")]
    public int FieldRecording
    {
        get => _fieldRecording;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _fieldRecording, value);
        }
    }

    [DataMember(Name = "interview")]
    [JsonPropertyName("interview")]
    public int Interview
    {
        get => _interview;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _interview, value);
        }
    }

    [DataMember(Name = "live")]
    [JsonPropertyName("live")]
    public int Live
    {
        get => _live;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _live, value);
        }
    }

    [DataMember(Name = "mixtape")]
    [JsonPropertyName("mixtape")]
    public int Mixtape
    {
        get => _mixtape;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _mixtape, value);
        }
    }

    [DataMember(Name = "remix")]
    [JsonPropertyName("remix")]
    public int Remix
    {
        get => _remix;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _remix, value);
        }
    }

    [DataMember(Name = "soundtrack")]
    [JsonPropertyName("soundtrack")]
    public int Soundtrack
    {
        get => _soundtrack;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _soundtrack, value);
        }
    }

    [DataMember(Name = "spokenword")]
    [JsonPropertyName("spokenword")]
    public int Spokenword
    {
        get => _spokenword;
        set
        {
            value = Math.Min(100, value);
            value = Math.Max(0, value);
            this.RaiseAndSetIfChanged(ref _spokenword, value);
        }
    }

    public object Clone()
    {
        return new FormatSettings()
        {
            _album = this._album,
            _single = this._single,
            _ep = this._ep,
            _other = this._other,
            _broadcast = this._broadcast,
            _audiobook = this._audiobook,
            _djMix = this._djMix,
            _audioDrama = this._audioDrama,
            _compilation = this._compilation,
            _spokenword = this._spokenword,
            _demo = this._demo,
            _fieldRecording = this._fieldRecording,
            _interview = this._interview,
            _live = this._live,
            _mixtape = this._mixtape,
            _remix = this._remix,
            _soundtrack = this._soundtrack
        };
    }
}