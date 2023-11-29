using System.Text;

namespace CustomXMLSerializer.Core;

public class XmlStringBuilder
{
    private readonly FileStream _fileStream;
    
    public string WriteStartElement(string elementName, IEnumerable<ElementAttribute>? attributes, bool closedElement = false)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"<{elementName}");

        if (attributes != null)
        {
            foreach (ElementAttribute attribute in attributes)
            {
                if (attribute.AttributeValue is null)
                {
                    continue;
                }

                stringBuilder.Append(WriteAttributeString(attribute.AttributeName, attribute.AttributeValue));
            }
        }
        
        string closingChar = closedElement ? "/>" : ">";
        stringBuilder.Append(closingChar);

        string result = stringBuilder.ToString();

        return result;
    }
    
    public string WriteEndElement(string elementName)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"</{elementName}>");
        return stringBuilder.ToString();
    }
    
    public string WriteAttributeString(string attributeName, string? attributeValue)
    {
        return $" {attributeName}=\"{attributeValue}\"";
    }
}