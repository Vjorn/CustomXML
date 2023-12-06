using System.Text;
using CustomXMLSerializer.Core.Attributes.Helpers;

namespace CustomXMLSerializer.Core.Helpers;

public static class XmlStringBuilder
{
    public static void AppendElementWithValue(this StringBuilder stringBuilder, string? elementName, string elementValue)
    {
        stringBuilder.Append(WriteStartElement(elementName));
        stringBuilder.Append(elementValue);
        stringBuilder.Append(WriteEndElement(elementName));
    }

    public static void AppendElementStart(this StringBuilder stringBuilder, string? elementName)
    {
        stringBuilder.Append(WriteStartElement(elementName));
    }
    
    public static void AppendElementEnd(this StringBuilder stringBuilder, string? elementName)
    {
        stringBuilder.Append(WriteEndElement(elementName));
    }
    
    public static string WriteStartElement(string? elementName, bool hasAttributes = false)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"<{elementName}");
        if (!hasAttributes)
        {
            stringBuilder.Append($">");
        }
        return stringBuilder.ToString();
    }
    
    public static string WriteEndElement(string? elementName)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"</{elementName}>");
        return stringBuilder.ToString();
    }
    
    public static string WriteAttributeString(string? attributeName, string? attributeValue)
    {
        return $" {attributeName}=\"{attributeValue}\"";
    }
}