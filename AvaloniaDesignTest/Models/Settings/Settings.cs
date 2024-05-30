﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AvaloniaDesignTest.Models.Settings;

public class Settings : ICloneable
{
    public static Settings GlobalSettings = new Settings();

    [JsonPropertyName("general-settings")] 
    public GeneralSettings GeneralSettings { get; set; } = new GeneralSettings();

    [JsonPropertyName("request-settings")]
    public RequestSettings RequestSettings { get; set; } = new RequestSettings();
    
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
        await JsonSerializer.SerializeAsync(stream, obj);
    }
}