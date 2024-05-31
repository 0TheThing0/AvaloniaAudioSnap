using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ReactiveUI;

namespace AvaloniaDesignTest.Models.Settings;

[DataContract]
public class Settings : ReactiveObject, ICloneable
{
    
    public static Settings GlobalSettings = new Settings();


    private GeneralSettings _generalSettings =  new GeneralSettings();
    private RequestSettings _requestSettings = new RequestSettings();
    [DataMember(Name="general-settings")] 
    public GeneralSettings GeneralSettings {
        get => _generalSettings;
        set => this.RaiseAndSetIfChanged(ref _generalSettings,value); } 

    [DataMember(Name="request-settings")]
    public RequestSettings RequestSettings{
        get => _requestSettings;
        set => this.RaiseAndSetIfChanged(ref _requestSettings,value); } 
    
    public object Clone()
    {
        return new Settings()
        {
            GeneralSettings = GeneralSettings.Clone() as GeneralSettings,
            RequestSettings = RequestSettings.Clone() as RequestSettings
        };
    }


    public async Task Save()
    {
        using (var fs = File.Create("appsettings.json"))
        {
            await SaveToStreamAsync(this, fs);
        }
    }

    private static async Task SaveToStreamAsync(Settings obj, Stream stream)
    {
       
        var ser = new DataContractJsonSerializer(typeof(Settings));
        ser.WriteObject(stream, obj);
    }
}