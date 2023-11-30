using CustomXMLSerializer.Core.Attributes;

namespace CustomXMLSerializer.Models.Parts;

public class InfoPart
{
    /// <summary>
    /// Уникальный идентификатор записи в ФКИ
    /// </summary>
    [CustomXmlAttribute(ElementName = "recnumber")]
    public string RecordNumber { get; set; }
}