using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Nodes;

namespace AvaloniaDesignTest.ViewModels;

public class MetadataUnit
{
    private PropertyInfo? _property;
    private string _newValue;
    public string Name { get; set; }
    public string OldValue { get; }
    public string NewValue
    {
        get => _newValue;
        set
        {
            if (_property?.PropertyType == typeof(uint) && !UInt32.TryParse(value, out _))
            {
                throw new ArgumentException(nameof(NewValue), "Not valid number");
            }
            _newValue = value;
        }
    }

    public MetadataUnit(string name, string oldValue, string newValue, PropertyInfo? property = null)
    {
        _property = property;
        Name = name;
        OldValue = oldValue;
        NewValue = newValue;
    }
}