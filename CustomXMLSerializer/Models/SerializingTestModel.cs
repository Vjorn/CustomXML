using CustomXMLSerializer.Core.Attributes;
using CustomXMLSerializer.Models.Parts;

namespace CustomXMLSerializer.Models;

[CustomXmlRoot(ElementName = "fch")]
public class SerializingTestModel
{
    [CustomXmlAttribute(ElementName = "version", DefaultValue = "4.0.1")] 
    public string Version { get; set; }
    
    [CustomXmlElement(ElementName = "head")] 
    public Head Head { get; set; }
    
    [CustomXmlElement(ElementName = "info")]
    public IEnumerable<InfoPart> InfoPart { get; set; }
    
    [CustomXmlElement(ElementName = "footer")]
    public Footer Footer { get; set; }
}

