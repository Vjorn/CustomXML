using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using CustomXMLSerializer.Core.Attributes;

namespace CustomXMLSerializer.Models;


[CustomXmlRoot(ElementName = "root_name")]
public class SerializingTestModel
{
    [CustomXmlAttribute(ElementName = "version")] public string Version { get; set; } = "4.0.1";
    [CustomXmlAttribute(ElementName = "attribute1")] public string? Attribute1 { get; set; }
    [CustomXmlAttribute(ElementName = "attribute2")] public string? Attribute2 { get; set; }
    [CustomXmlAttribute(ElementName = "attribute3")] public string? Attribute3 { get; set; }
    [CustomXmlAttribute(ElementName = "attribute4")] public string? Attribute4 { get; set; }
    [CustomXmlAttribute(ElementName = "attribute5")] public string? Attribute5 { get; set; }
    
    [CustomXmlHeaderElement(ElementName = "header1")] public Header1 Header1 { get; set; }
    [CustomXmlHeaderElement(ElementName = "header2")] public Header2 Header2 { get; set; }
    [CustomXmlHeaderElement(ElementName = "header3")] public Header3 Header3 { get; set; }
    
    // [XmlElement(ElementName = "info")] public List<InfoPart> InfoBlock { get; set; }
    [CustomXmlFooterElement(ElementName = "footer1")] public Footer1 Footer1 { get; set; }
    [CustomXmlFooterElement(ElementName = "footer2")] public Footer2 Footer2 { get; set; }
    [CustomXmlFooterElement(ElementName = "footer3")] public Footer3 Footer3 { get; set; }
}

public sealed class Header1
{
    [CustomXmlElement(ElementName = "header1_string_element1")] public string Header1_StringElement1 { get; set; }
}

public sealed class Header2
{
    [CustomXmlElement(ElementName = "header2_string_element1")] public string Header2_StringElement1 { get; set; }
}

public sealed class Header3
{
    [CustomXmlElement(ElementName = "header3_string_element1")] public string Header3_StringElement1 { get; set; }
}

public sealed class Footer1
{
    [CustomXmlElement(ElementName = "footer1_string_element1")] public string Footer1_StringElement1 { get; set; }
}

public sealed class Footer2
{
    [CustomXmlElement(ElementName = "footer2_string_element1")] public string Footer2_StringElement1 { get; set; }
}

public sealed class Footer3
{
    [CustomXmlElement(ElementName = "footer3_string_element1")] public string Footer3_StringElement1 { get; set; }
}














/// <summary>
/// ///////////
/// </summary>

public sealed class Head
{
    [Required]
    [MinLength(10)]
    [MaxLength(12)]
    [RegularExpression("([0-9]{10})|([0-9]{12})")]
    [XmlElement(ElementName = "source_inn")] public string SourceInn { get; set; }

    [Required]
    [MinLength(13)]
    [MaxLength(15)]
    [RegularExpression("([0-9]{13})|([0-9]{15})")]
    [XmlElement(ElementName = "source_ogrn")] public string SourceOgrn { get; set; }

    [XmlElement(ElementName = "date")] public DateTime Date { get; set; }

    [Required]
    [XmlElement(ElementName = "file_reg_date")] public DateTime FileRegDate { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    [XmlElement(ElementName = "file_reg_num")] public string FileRegNum { get; set; }
    [XmlElement(ElementName = "prev_file")] public PrevFile PrevFile { get; set; }
}

public sealed class PrevFile
{
    [XmlElement(ElementName = "file_reg_date")] public DateTime FileRegDate { get; set; }
    [XmlElement(ElementName = "file_reg_num")] public string FileRegNum { get; set; }
}

public class Footer
{
    [XmlElement(ElementName = "subjects_count")] public int SubjectsCount { get; set; }
    [XmlElement(ElementName = "records_count")] public int RecordsCount { get; set; }
}

public sealed class InfoPart
{
    [XmlIgnore] public int PriorVstupId { get; set; }
    
    [MinLength(1)]
    [MaxLength(50)]
    [XmlAttribute(AttributeName = "recnumber")] public int RecordNumber { get; set; }
    
    [MinLength(1)]
    [MaxLength(10)]
    [XmlAttribute(AttributeName = "event")] public string Event { get; set; }
    [XmlElement(ElementName = "title_part")] public TitlePart TitlePart { get; set; }
}

public class TitlePart
{
    [XmlElement(ElementName = "kski")] public SCode Kski { get; set; } = null;

    [XmlElement(ElementName = "private")] public Private Private { get; set; }
}

public class SCode
{
    [XmlElement(ElementName = "code")] public string Code { get; set; }
}

public sealed class Private
{
    [XmlElement(ElementName = "name")] public NamePrivate Name { get; set; }
    [XmlElement(ElementName = "doc")] public List<Doc> Doc { get; set; }
    [XmlElement(ElementName = "birth")] public Birth Birth { get; set; }
}

public sealed class NamePrivate
{
    [XmlElement(ElementName = "last")] public string Last { get; set; }
    [XmlElement(ElementName = "first")] public string First { get; set; }
    [XmlElement(ElementName = "middle")] public string Middle { get; set; }
}

public class Birth
{
    [XmlElement(ElementName = "date")] public string BirthDate { get; set; }
    [XmlElement(ElementName = "country")] public string CountryCode { get; set; }
    [XmlElement(ElementName = "place")] public string BirthPlace { get; set; }
}

public sealed class Doc
{
    [XmlElement(ElementName = "country")] public string Country { get; set; }
    [XmlElement(ElementName = "country_text")] public string CountryText { get; set; }
    [XmlElement(ElementName = "type")] public string Type { get; set; }
    [XmlElement(ElementName = "type_text")] public string TypeText { get; set; }
    [XmlElement(ElementName = "serial")] public string Serial { get; set; }
    [XmlElement(ElementName = "number")] public string Number { get; set; }
    [XmlElement(ElementName = "date")] public string Date { get; set; }
    [XmlElement(ElementName = "who")] public string Who { get; set; }
    
    [StringLength(7)] 
    [RegularExpression(@"^[0-9]{3}-[0-9]{3}$")] 
    [XmlElement(ElementName = "department_code")] public string DepartmentCode { get; set; }

    [XmlElement(ElementName = "end_date")] public string EndDate { get; set; }
}