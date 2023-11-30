namespace CustomXMLSerializer.Models;

public class ElementInfo
{
    public string? Name { get; set; }
    public ElementType Type { get; set; }

    public ElementInfo(string elementName, ElementType elementType)
    {
        Name = elementName;
        Type = elementType;
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