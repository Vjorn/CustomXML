using System.Text;
using CustomXMLSerializer.Core.Attributes.Helpers;
using CustomXMLSerializer.Core.Helpers;
using CustomXMLSerializer.Models;
using CustomXMLSerializer.Models.Parts;

namespace CustomXMLSerializer.Data;

public class TestDataBuilder
{
    public SerializingTestModel BuildData()
    {
        string filePath = "text.xml";
        SerializingTestModel model = new SerializingTestModel();

        ModelInfoCollector modelInfoCollector = new ModelInfoCollector(model.GetType());
        
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            StringBuilder stringBuilder = new StringBuilder();

            #region Open Root Element

            string versionAttributeKey = $"{model.GetType().Name}.{nameof(model.Version)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetRootElementInfo().Name, true));
            stringBuilder.Append(XmlStringBuilder.WriteAttributeString(modelInfoCollector.GetAttributeByKey(versionAttributeKey).Name,
                modelInfoCollector.GetAttributeByKey(versionAttributeKey).DefaultValue));
            stringBuilder.Append(">");

            #endregion
            
            stringBuilder.Append(GetHeaderString(model, modelInfoCollector)); // Append header
            GetInfoParts(stringBuilder, model, modelInfoCollector); // Fill info part
            stringBuilder.Append(GetFooterString(model, modelInfoCollector)); // Append footer
            
            
            #region Close Root Element

            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetRootElementInfo().Name));

            #endregion

            string result = stringBuilder.ToString();
            
            byte[] startRootElementBytes = Encoding.UTF8.GetBytes(result);
            
            fileStream.Write(startRootElementBytes, 0, startRootElementBytes.Length);
            fileStream.Flush();
        }
        return model;
    }

    private string GetHeaderString(SerializingTestModel model, ModelInfoCollector modelInfoCollector)
    {
        StringBuilder headerStringBuilder = new StringBuilder();

        string headElementKey = $"{model.GetType().Name}.{nameof(model.Head)}";
        headerStringBuilder.AppendElementStart(modelInfoCollector.GetElementByKey(headElementKey).Name);

        headerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{headElementKey}.{nameof(model.Head.SourceInn)}").Name,
            "SourceInn Value");
        headerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{headElementKey}.{nameof(model.Head.SourceOgrn)}").Name,
            "SourceOgrn Value");
        headerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{headElementKey}.{nameof(model.Head.Date)}").Name,
            "Date Value");
        headerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{headElementKey}.{nameof(model.Head.FileRegDate)}").Name,
            "FileRegDate Value");
        headerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{headElementKey}.{nameof(model.Head.FileRegNum)}").Name,
            "FileRegNum Value");

        string headPrevFileElementKey = $"{headElementKey}.{nameof(model.Head.PrevFile)}";
        headerStringBuilder.AppendElementStart(modelInfoCollector.GetElementByKey(headPrevFileElementKey).Name);

        headerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{headPrevFileElementKey}.{nameof(model.Head.PrevFile.FileRegDate)}").Name,
            "PrevFile.FileRegDate Value");
        headerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{headPrevFileElementKey}.{nameof(model.Head.PrevFile.FileRegNum)}").Name,
            "PrevFile.FileRegNum Value");

        headerStringBuilder.AppendElementEnd(modelInfoCollector.GetElementByKey(headPrevFileElementKey).Name);
        
        headerStringBuilder.AppendElementEnd(modelInfoCollector.GetElementByKey(headElementKey).Name);
        
        return headerStringBuilder.ToString();
    }

    private string GetFooterString(SerializingTestModel model, ModelInfoCollector modelInfoCollector)
    {
        StringBuilder footerStringBuilder = new StringBuilder();

        string footerElementKey = $"{model.GetType().Name}.{nameof(model.Footer)}";
        footerStringBuilder.AppendElementStart(modelInfoCollector.GetElementByKey(footerElementKey).Name);
        
        footerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{footerElementKey}.{nameof(model.Footer.SubjectsCount)}").Name,
            "999");
        
        footerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{footerElementKey}.{nameof(model.Footer.RecordsCount)}").Name,
            "999");
        
        footerStringBuilder.AppendElementEnd(modelInfoCollector.GetElementByKey(footerElementKey).Name);
        
        return footerStringBuilder.ToString();
    }

    private void GetInfoParts(StringBuilder stringBuilder, SerializingTestModel model, ModelInfoCollector modelInfoCollector)
    {
        for (int i = 0; i < 5; i++)
        {
            string infoPartElementKey = $"{model.GetType().Name}.{nameof(model.InfoPart)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(infoPartElementKey).Name, true));
            
            string infoPartRecordNumberAttributeKey = $"{infoPartElementKey}.{nameof(InfoPart.RecordNumber)}";
            stringBuilder.Append(XmlStringBuilder.WriteAttributeString(modelInfoCollector.GetAttributeByKey(infoPartRecordNumberAttributeKey).Name,
                $"Recnumber is {i}"));
            
            stringBuilder.Append(">");
            
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(infoPartElementKey).Name));
        }
    }
}