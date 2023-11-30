using CustomXMLSerializer.Core.Attributes;

namespace CustomXMLSerializer.Models.Parts;

public sealed class Head
{
    [CustomXmlElement(ElementName = "source_inn")]
    public string SourceInn { get; set; }
    
    [CustomXmlElement(ElementName = "source_ogrn")]
    public string SourceOgrn { get; set; }

    [CustomXmlElement(ElementName = "date")]
    public string Date { get; set; }
    
    [CustomXmlElement(ElementName = "file_reg_date")]
    public string FileRegDate { get; set; }
    
    [CustomXmlElement(ElementName = "file_reg_num")]
    public string FileRegNum { get; set; }

    [CustomXmlElement(ElementName = "prev_file")]
    public PrevFile PrevFile { get; set; }
}

public sealed class PrevFile
{
    [CustomXmlElement(ElementName = "file_reg_date")]
    public string FileRegDate { get; set; }

    [CustomXmlElement(ElementName = "file_reg_num")]
    public string FileRegNum { get; set; }
}