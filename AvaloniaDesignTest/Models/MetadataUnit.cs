using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using AvaloniaDesignTest.Models.Settings;
using ReactiveUI;
using TagLib;

namespace AvaloniaDesignTest.ViewModels;

public class MetadataUnit : ReactiveObject
{
    private string _newValue;
    private string _oldValue;
    [JsonPropertyName("field-name")]
    public string FieldName { get; set; }
    [JsonIgnore]
    public string Name { get; set; }
    [JsonPropertyName("old-value")]
    public string OldValue  {
        get => _oldValue;
        set => this.RaiseAndSetIfChanged(ref _oldValue, value);
    }

    [JsonIgnore]
    public PropertyInfo? Property
    {
        get => _property;
        set => this.RaiseAndSetIfChanged(ref _property, value);
    }

    private PropertyInfo? _property;
    [JsonPropertyName("new-value")]
    public string NewValue
    {
        get => _newValue;
        set
        {
            if (value!="" && Property?.PropertyType == typeof(uint) && !UInt32.TryParse(value, out _))
            {
                throw new ArgumentException(nameof(NewValue), "Not valid number");
            }

            this.RaiseAndSetIfChanged(ref _newValue, value);
        }
    }
    public MetadataUnit(string name, string oldValue, string newValue, PropertyInfo property)
    {
        Property = property;
        Name = name;
        if (Settings.GlobalSettings.GeneralSettings.MetadataNameParity.TryGetValue(name, out var value))
        {
            Name = value;
        }
        
        FieldName = name;
        OldValue = oldValue;
        NewValue = newValue;
    }

    public MetadataUnit()
    {
        
    }

    public void UpdatePropertyInfo()
    {
        Property= null;
        MetadataSettings.TagFieldParity.TryGetValue(FieldName,out _property);
        
        Name = FieldName;
        if (Settings.GlobalSettings.GeneralSettings.MetadataNameParity.TryGetValue(FieldName, out var value))
        {
            Name = value;
        }
        
        if (_property == null || !_property.CanWrite || !_property.CanRead)
        {
            Property = null;
        }
    }
    public static string ConvertToString(object value)
    {
        if (value is string[] strArrayValue)
        {
            return String.Join("; ", strArrayValue);
        }
        if (value is string strValue)
        {
            return strValue;
        }
        if (value is uint uintValue)
        {
            return uintValue.ToString();
        }

        if (value is JsonValue jsonValue)
        {
            if (jsonValue.TryGetValue(out uint jsonVal))
            {
                return value.ToString();
            }
            if (jsonValue.TryGetValue(out string str))
            {
                return str;
            }
        }

        if (value is JsonArray jsonArray)
        {
            List<string> strList = new List<string>();
            foreach (var nodeArray in jsonArray)
            {
                strList.Add(ConvertToString(nodeArray));
            }
            return String.Join("; ", strList);
        }

        return "";
    }

    public void ApplyChange(Tag tag)
    {
       Property.SetValue((Tag)tag, StringToSaveType(NewValue, Property.PropertyType));
    }
    
    public static object StringToSaveType(string value, Type conversionType)
    {
        if (conversionType == typeof(string[]))
        {
            return value.Split("; ");
        }
        if (conversionType == typeof(uint))
        {
            if (value == "")
                return 0u;
            return Convert.ToUInt32(value);
        }
        if (conversionType == typeof(string))
        {
            return value;
        }
        return null;
    }
}