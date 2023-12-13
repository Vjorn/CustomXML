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
    
    /// <summary>
    /// Here position to store records/events as info parts
    /// </summary>
    
    [CustomXmlElement(ElementName = "footer")]
    public Footer Footer { get; set; }
}

