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
    public int Album  {
        get => _album;
        set => this.RaiseAndSetIfChanged(ref _album,value);
    } 
    [DataMember(Name="single")]
    public int Single  {
        get => _single;
        set => this.RaiseAndSetIfChanged(ref _single,value);
    }  
    [DataMember(Name="ep")]
    public int EP {
        get => _ep;
        set => this.RaiseAndSetIfChanged(ref _ep,value);
    }  
    [DataMember(Name="other")]
    public int Other {
        get => _other;
        set => this.RaiseAndSetIfChanged(ref _other,value);
    }  
    [DataMember(Name="broadcast")]
    public int Broadcast {
        get => _broadcast;
        set => this.RaiseAndSetIfChanged(ref _broadcast,value);
    }  
    [DataMember(Name="audio-drama")]
    public int AudioDrama {
        get => _audioDrama;
        set => this.RaiseAndSetIfChanged(ref _audioDrama,value);
    }  
    [DataMember(Name="audiobook")]
    public int Audiobook {
        get => _audiobook;
        set => this.RaiseAndSetIfChanged(ref _audiobook,value);
    } 
    [DataMember(Name="compilation")]
    public int Compilation {
        get => _compilation;
        set => this.RaiseAndSetIfChanged(ref _compilation,value);
    }  
    [DataMember(Name="dj-mix")]
    public int DjMix {
        get => _djMix;
        set => this.RaiseAndSetIfChanged(ref _djMix,value);
    }  
    [DataMember(Name="demo")]
    public int Demo {
        get => _demo;
        set => this.RaiseAndSetIfChanged(ref _demo,value);
    }
    [DataMember(Name="field-recording")]
    public int FieldRecording {
        get => _fieldRecording;
        set => this.RaiseAndSetIfChanged(ref _fieldRecording,value);
    }  
    [DataMember(Name="interview")]
    public int Interview {
        get => _interview;
        set => this.RaiseAndSetIfChanged(ref _interview,value);
    }  
    [DataMember(Name="live")]
    public int Live {
        get => _live;
        set => this.RaiseAndSetIfChanged(ref _live,value);
    } 
    [DataMember(Name="mixtape")]
    public int Mixtape {
        get => _mixtape;
        set => this.RaiseAndSetIfChanged(ref _mixtape,value);
    }  
    [DataMember(Name="remix")]
    public int Remix {
        get => _remix;
        set => this.RaiseAndSetIfChanged(ref _remix,value);
    }  
    [DataMember(Name="soundtrack")]
    public int Soundtrack {
        get => _soundtrack;
        set => this.RaiseAndSetIfChanged(ref _soundtrack,value);
    }  
    [DataMember(Name="spokenword")]
    public int Spokenword {
        get => _spokenword;
        set => this.RaiseAndSetIfChanged(ref _spokenword,value);
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