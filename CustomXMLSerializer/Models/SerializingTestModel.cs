using CustomXMLSerializer.Core.Attributes;

namespace CustomXMLSerializer.Models;

[CustomXmlRoot(ElementName = "test_root")]
public class SerializingTestModel
{
    [CustomXmlAttribute(ElementName = "version")] public string Version { get; set; } = "4.0.1";
    [CustomXmlAttribute(ElementName = "RootAttribute1")] public string RootAttribute1 { get; set; }
    [CustomXmlAttribute(ElementName = "RootAttribute2")] public string RootAttribute2 { get; set; }
    [CustomXmlHeaderElement(ElementName = "headerString")] public string HeaderString { get; set; }
    [CustomXmlHeaderElement(ElementName = "headerClass")] public Header HeaderClass { get; set; }
    [CustomXmlHeaderElement(ElementName = "headerBlock")] public IEnumerable<Header> HeaderBlock { get; set; }
    
    [CustomXmlElement(ElementName = "info_string")] public string InfoString { get; set; }
    [CustomXmlElement(ElementName = "info_class")] public InfoPart InfoClass { get; set; }
    [CustomXmlElement(ElementName = "info_block")] public IEnumerable<InfoPart> InfoBlock { get; set; }
}

public class Header
{
    [CustomXmlElement(ElementName = "header1_string_element1")] public string Header1 { get; set; }
    [CustomXmlElement(ElementName = "header1_string_element2")] public string Header2 { get; set; }
    [CustomXmlElement(ElementName = "header1_string_element3")] public string Header3 { get; set; }
    [CustomXmlElement(ElementName = "header1_string_element3")] public SubHeader Header4 { get; set; }
}

public class SubHeader
{
    [CustomXmlAttribute(ElementName = "subHeader1_string_attribute1")] public string SubHeaderAttribute1 { get; set; }
    [CustomXmlAttribute(ElementName = "subHeader1_string_attribute2")] public string SubHeaderAttribute2 { get; set; }
    [CustomXmlElement(ElementName = "subHeader1_string_element1")] public string SubHeader1 { get; set; }
    [CustomXmlElement(ElementName = "subHeader1_string_element2")] public string SubHeader2 { get; set; }
}

public class InfoPart
{
    [CustomXmlElement(ElementName = "InfoPart_attribute1")] public string InfoPartAttribute1 { get; set; }
    [CustomXmlElement(ElementName = "InfoPart_attribute2")] public string InfoPartAttribute2 { get; set; }
    [CustomXmlElement(ElementName = "InfoPart_element1")] public string InfoPartElement1 { get; set; }
    [CustomXmlElement(ElementName = "InfoPart_element2")] public string InfoPartElement2 { get; set; }
    [CustomXmlElement(ElementName = "InfoPart_element3")] public string InfoPartElement3 { get; set; }
}