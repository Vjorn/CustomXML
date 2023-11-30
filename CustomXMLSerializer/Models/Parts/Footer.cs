using CustomXMLSerializer.Core.Attributes;

namespace CustomXMLSerializer.Models.Parts;

public class Footer
{
    [CustomXmlElement(ElementName = "subjects_count")]
    public string? SubjectsCount { get; set; }

    [CustomXmlElement(ElementName = "records_count")]
    public string? RecordsCount { get; set; }
}