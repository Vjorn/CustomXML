using System.Text;
using CustomXMLSerializer.Core.Attributes.Helpers;
using CustomXMLSerializer.Core.Helpers;
using CustomXMLSerializer.Models.Parts.Events.SubParts;

namespace CustomXMLSerializer.Data.Builders;

public static class TitlePartBuilder
{
    public static string GetTitlePartString()
    {
        TitlePart titlePartModel = new TitlePart();
        
        Type titlePartType = titlePartModel.GetType();
        ModelInfoCollector titlePartModelInfoCollector = new ModelInfoCollector(titlePartType);

        StringBuilder titleStringBuilder = new StringBuilder();

        string titlePartRootKey = titlePartType.Name;
        string privateElementKey = $"{titlePartRootKey}.{nameof(titlePartModel.Private)}";
        string privateNameElementKey = $"{privateElementKey}.{nameof(titlePartModel.Private.Name)}";
        
        titleStringBuilder.Append(
            XmlStringBuilder.WriteStartElement(titlePartModelInfoCollector.GetRootElementInfo().Name));
        
        titleStringBuilder.AppendElementStart(titlePartModelInfoCollector.GetElementByKey(privateElementKey).Name);

        titleStringBuilder.AppendElementStart(titlePartModelInfoCollector.GetElementByKey(privateNameElementKey).Name);

        titleStringBuilder.AppendElementWithValue(
            titlePartModelInfoCollector.GetElementByKey($"{privateNameElementKey}.{nameof(titlePartModel.Private.Name.Last)}").Name,
            "Пророк");
        
        titleStringBuilder.AppendElementWithValue(
            titlePartModelInfoCollector.GetElementByKey($"{privateNameElementKey}.{nameof(titlePartModel.Private.Name.First)}").Name,
            "Санбой");
        
        titleStringBuilder.AppendElementWithValue(
            titlePartModelInfoCollector.GetElementByKey($"{privateNameElementKey}.{nameof(titlePartModel.Private.Name.Middle)}").Name,
            "Легендарыч");
        
        titleStringBuilder.AppendElementEnd(titlePartModelInfoCollector.GetElementByKey(privateNameElementKey).Name);
        
        
        titleStringBuilder.AppendElementEnd(titlePartModelInfoCollector.GetElementByKey(privateElementKey).Name);

        titleStringBuilder.Append(
            XmlStringBuilder.WriteEndElement(titlePartModelInfoCollector.GetRootElementInfo().Name));
        
        
        return titleStringBuilder.ToString();
    }
}