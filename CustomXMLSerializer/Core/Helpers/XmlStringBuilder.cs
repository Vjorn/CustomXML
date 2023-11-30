using System.Text;

namespace CustomXMLSerializer.Core.Helpers;

public static class XmlStringBuilder
{
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