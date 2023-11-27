using System.Diagnostics.CodeAnalysis;

namespace CustomXMLSerializer.Core.Attributes;

public abstract class CustomAttribute : Attribute
{
    private string? _elementName;

    protected CustomAttribute()
    {
    }

    protected CustomAttribute(string attributeName)
    {
        this._elementName = attributeName;
    }
    
    [AllowNull]
    public string ElementName
    {
        get => _elementName ?? string.Empty;
        set => _elementName = value;
    }
}

public abstract class CustomAttributeAttribute : CustomAttribute
{
    
}

public abstract class CustomElementAttribute : CustomAttribute
{
    
}