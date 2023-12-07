using CustomXMLSerializer.Core.Attributes;

namespace CustomXMLSerializer.Models.Parts.Events;

[CustomXmlRoot(ElementName = "info")]
public class SubjectMainChangedEvent
{
    [CustomXmlAttribute(ElementName = "recnumber")]
    public string RecordNumber { get; set; }
    
    /// <summary>
    /// Номер события, вследствие которого формируется запись кредитной истории
    /// </summary>
    [CustomXmlAttribute(ElementName = "event")]
    public string Event { get; set; }
    
    /// <summary>
    /// Дата наступления События, вследствие которого формируется запись кредитной истории
    /// </summary>
    [CustomXmlAttribute(ElementName = "event_date")]
    public string EventDate { get; set; }
    
    /// <summary>
    /// Код операции, производимой с записью кредитной истории
    /// </summary>
    [CustomXmlAttribute(ElementName = "action")]
    public string Action { get; set; }
    
    /// <summary>
    /// Объём выполняемой операции, производимой с записью кредитной истории
    /// </summary>
    [CustomXmlAttribute(ElementName = "action_volume")]
    public string ActionVolume { get; set; }

    /// <summary>
    /// Исходящая регистрационная дата файла (для ранее отбракованной записи)
    /// </summary>
    [CustomXmlAttribute(ElementName = "prev_file_reg_date")]
    public string PrevFileRegDate { get; set; }

    /// <summary>
    /// Исходящая регистрационный номер файла (для ранее отбракованной записи)
    /// </summary>
    [CustomXmlAttribute(ElementName = "prev_file_reg_num")]
    public string PrevFileRegNum { get; set; }
}