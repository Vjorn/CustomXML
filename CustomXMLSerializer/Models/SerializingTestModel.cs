using CustomXMLSerializer.Core.Attributes;

namespace CustomXMLSerializer.Models;

[CustomXmlRoot(ElementName = "test_root")]
public class SerializingTestModel
{
    [CustomXmlAttribute(ElementName = "version")] public string Version { get; set; } = "4.0.1";
    [CustomXmlAttribute(ElementName = "RootAttribute1")] public string RootAttribute1 { get; set; }
    [CustomXmlHeaderElement(ElementName = "headerString")] public string HeaderString { get; set; }
    [CustomXmlHeaderElement(ElementName = "headerClass")] public Header HeaderClass { get; set; }
    [CustomXmlHeaderElement(ElementName = "headerBlock")] public IEnumerable<Header> HeaderBlock { get; set; }
    
    
    public IEnumerable<InfoPart> InfoBlock { get; set; }
}

public class Header
{
    [CustomXmlElement(ElementName = "header1_string_element1")] public string Header1 { get; set; }
    [CustomXmlElement(ElementName = "header1_string_element2")] public string Header2 { get; set; }
    [CustomXmlElement(ElementName = "header1_string_element3")] public string Header3 { get; set; }
}

public class InfoPart
{
    public string InfoPartElement1 { get; set; }
    public string InfoPartElement2 { get; set; }
    public string InfoPartElement3 { get; set; }
}