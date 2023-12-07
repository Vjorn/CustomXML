using CustomXMLSerializer.Core.Attributes;

namespace CustomXMLSerializer.Models.Parts.Events.SubParts;

public class TitlePart
{
    [CustomXmlElement(ElementName = "kski")] 
    public SCode Kski { get; set; } = null;

    [CustomXmlElement(ElementName = "private")]
    public Private Private { get; set; }
}

public class SCode
{
    [CustomXmlElement(ElementName = "code")]
    public string Code { get; set; }
}

public sealed class Private
{
    [CustomXmlElement(ElementName = "name")]
    public NamePrivate Name { get; set; }

    [CustomXmlElement(ElementName = "doc")]
    public List<Doc> Doc { get; set; }

    [CustomXmlElement(ElementName = "birth")]
    public Birth Birth { get; set; }

    [CustomXmlElement(ElementName = "history")]
    public List<HistoryPrivate> History { get; set; }

    [CustomXmlElement(ElementName = "inn")]
    public InnPrivate Inn { get; set; }

    [CustomXmlElement(ElementName = "snils")]
    public Snils Snils { get; set; }
}

public sealed class NamePrivate
{
    [CustomXmlElement(ElementName = "last", ChangeableElement = true)]
    public string Last { get; set; }

    [CustomXmlElement(ElementName = "first", ChangeableElement = true)]
    public string First { get; set; }

    [CustomXmlElement(ElementName = "middle", ChangeableElement = true)]
    public string Middle { get; set; }
}

public class Birth
{
    [CustomXmlElement(ElementName = "date", ChangeableElement = true)]
    public string BirthDate { get; set; }

    [CustomXmlElement(ElementName = "country")]
    public string CountryCode { get; set; }

    [CustomXmlElement(ElementName = "place", ChangeableElement = true)]
    public string BirthPlace { get; set; }
}

public class HistoryPrivate
{
    [CustomXmlElement(ElementName = "name")]
    public List<HistoryName> Name { get; set; }

    [CustomXmlElement(ElementName = "doc")]
    public List<HistoryDoc> Doc { get; set; }
}

public class InnPrivate
{
    [CustomXmlElement(ElementName = "code", ChangeableElement = true)]
    public string Code { get; set; }

    [CustomXmlElement(ElementName = "no", ChangeableElement = true)]
    public string No { get; set; }
    
    // Признак специального налогового режима (ФЛ_6.4)
    // 0 = обстоятельство кода «1» отсутствует
    // 1 = субъект применяет специальный налоговый
    // режим «Налог на профессиональный доход» в соответствии с Федеральным
    // законом от 27 ноября 2018 года № 422-ФЗ «О проведении эксперимента по
    // установлению специального налогового режима «Налог на профессиональный 
    // доход»
    [CustomXmlElement(ElementName = "special_tax_mode")]
    public string SpecialTaxMode { get; set; }
}

public class Snils
{
    [CustomXmlElement(ElementName = "no", ChangeableElement = true)]
    public string No { get; set; }
}

public sealed class Doc
{
    [CustomXmlElement(ElementName = "country")]
    public string Country { get; set; }

    [CustomXmlElement(ElementName = "country_text")]
    public string CountryText { get; set; }

    [CustomXmlElement(ElementName = "type")]
    public string Type { get; set; }

    [CustomXmlElement(ElementName = "type_text")]
    public string TypeText { get; set; }

    [CustomXmlElement(ElementName = "serial", ChangeableElement = true)]
    public string Serial { get; set; }

    [CustomXmlElement(ElementName = "number", ChangeableElement = true)]
    public string Number { get; set; }

    [CustomXmlElement(ElementName = "date", ChangeableElement = true)]
    public string Date { get; set; }

    [CustomXmlElement(ElementName = "who", ChangeableElement = true)]
    public string Who { get; set; }
    
    [CustomXmlElement(ElementName = "department_code", ChangeableElement = true)]
    public string DepartmentCode { get; set; }

    [CustomXmlElement(ElementName = "end_date")]
    public string EndDate { get; set; }
}

public class HistoryDoc
{
    /// <summary>
    /// Признак наличия документа
    /// </summary>
    [CustomXmlElement(ElementName = "hist_doc_sign")]
    public string HistDocSign { get; set; }
    
    /// <summary>
    /// Код страны гражданства по ОКСМ
    /// </summary>
    [CustomXmlElement(ElementName = "country")]
    public string CountryCode { get; set; }
    
    /// <summary>
    /// Наименование иной страны
    /// </summary>
    [CustomXmlElement(ElementName = "country_text")]
    public string CountryText { get; set; }
    
    /// <summary>
    /// Код документа
    /// </summary>
    [CustomXmlElement(ElementName = "type")]
    public string IdCode { get; set; }
    
    /// <summary>
    /// Наименование иного документа
    /// </summary>
    [CustomXmlElement(ElementName = "type_text")]
    public string OtherId { get; set; }
    
    /// <summary>
    /// Серия документа
    /// </summary>
    [CustomXmlElement(ElementName = "serial")]
    public string IdSeries { get; set; }
    
    /// <summary>
    /// Номер документа
    /// </summary>
    [CustomXmlElement(ElementName = "number")]
    public string IdNum { get; set; }
    
    /// <summary>
    /// Дата выдачи документа 
    /// </summary>
    [CustomXmlElement(ElementName = "date")]
    public string IssueDate { get; set; }
    
    /// <summary>
    /// Кем выдан документ
    /// </summary>
    [CustomXmlElement(ElementName = "who")]
    public string Issuer { get; set; }
    
    /// <summary>
    /// Код подразделения (Формат: «NNN-NNN»)
    /// </summary>
    [CustomXmlElement(ElementName = "department_code")]
    public string DeptCode { get; set; }
    
    /// <summary>
    /// Дата окончания срока действия документа
    /// </summary>
    [CustomXmlElement(ElementName = "end_date")]
    public string EndDate { get; set; }
}

public class HistoryName
{
    /// <summary>
    /// Признак наличия предыдущего имени
    /// </summary>
    [CustomXmlElement(ElementName = "hist_name_sign")]
    public string HistNameSign { get; set; }
    
    [CustomXmlElement(ElementName = "last")]
    public string LastName { get; set; }
    
    [CustomXmlElement(ElementName = "first")]
    public string FirstName { get; set; }

    [CustomXmlElement(ElementName = "middle")]
    public string MiddleName { get; set; }
       
    [CustomXmlElement(ElementName = "doc_date")]
    public string DocDate { get; set; }
}