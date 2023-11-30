namespace CustomXMLSerializer.Core;

public class XmlStreamWriter
{
    private readonly StreamWriter _writer;

    public XmlStreamWriter(StreamWriter writer)
    {
        this._writer = writer;
    }

    public void WriteStartElement(string elementName)
    {
        _writer.Write($"<{elementName}>");
        Flush();
    }
    
    public void WriteStartElement(string elementName, IEnumerable<ElementAttribute> attributes)
    {
        _writer.Write($"<{elementName}");
         foreach (ElementAttribute attribute in attributes)
         {
             WriteAttributeString(attribute.AttributeName, attribute.AttributeValue);
         }
        _writer.Write(">");
    }

    public void WriteAttributeString(string? attributeName, string attributeValue)
    {
        _writer.Write($" {attributeName}=\"{attributeValue}\"");
    }
    
    public void WriteEndElement(string elementName)
    {
        _writer.Write($"</{elementName}>");
    }

    public void WriteEndElement()
    {
        _writer.Write("</>");
    }

    public void WriteValue(string value)
    {
        _writer.Write(value);
    }

    public void Flush()
    {
        _writer.Flush();
    }
}

public class ElementAttribute
{
    public string? AttributeName { get; set; }
    public string? AttributeValue { get; set; }
}