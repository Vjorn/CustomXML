using System.Text;
using CustomXMLSerializer.Core.Attributes.Helpers;
using CustomXMLSerializer.Core.Helpers;
using CustomXMLSerializer.Models.Parts.Events.SubParts;

namespace CustomXMLSerializer.Data.Builders;

public static class TitlePartBuilder
{
    public static string GetTitlePartString()
    {
        Type titlePartType = typeof(TitlePart);
        ModelInfoCollector titlePartModelInfoCollector = new ModelInfoCollector(titlePartType);

        StringBuilder titleStringBuilder = new StringBuilder();
        
        titleStringBuilder.Append(
            XmlStringBuilder.WriteStartElement(titlePartModelInfoCollector.GetRootElementInfo().Name));


        titleStringBuilder.Append(
            XmlStringBuilder.WriteEndElement(titlePartModelInfoCollector.GetRootElementInfo().Name));
        
        
        return titleStringBuilder.ToString();
    }
}