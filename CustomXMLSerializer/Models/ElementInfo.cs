namespace CustomXMLSerializer.Models;

public class ElementInfo
{
    public string? Key { get; set; }
    public string? Name { get; set; }
    public ElementType Type { get; set; }
    public string? DefaultValue { get; set; }

    public ElementInfo(string elementKey, string? elementName, ElementType elementType)
    {
        Key = elementKey;
        Name = elementName;
        Type = elementType;
    }
    
    public ElementInfo(string elementKey, string? elementName, ElementType elementType, string? elementDefaultValue)
    {
        Key = elementKey;
        Name = elementName;
        Type = elementType;
        DefaultValue = elementDefaultValue;
    }
}

public enum ElementType
{
    Root,
    RootAttribute,
    HeaderElement,
    HeaderBlock,
    Element,
    Attribute
}