using System.Reflection;
using System.Text;
using CustomXMLSerializer.Core;
using CustomXMLSerializer.Core.Attributes;
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

            #region Open Header Element

            string headElementKey = $"{model.GetType().Name}.{nameof(model.Head)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(headElementKey).Name));

            #endregion

            #region Fill Header

            string headSourceInnElementKey = $"{headElementKey}.{nameof(model.Head.SourceInn)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(headSourceInnElementKey).Name));
            stringBuilder.Append("SourceInn Value");
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(headSourceInnElementKey).Name));
            
            string headSourceOgrnElementKey = $"{headElementKey}.{nameof(model.Head.SourceOgrn)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(headSourceOgrnElementKey).Name));
            stringBuilder.Append("SourceOgrn Value");
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(headSourceOgrnElementKey).Name));
            
            string headDateElementKey = $"{headElementKey}.{nameof(model.Head.Date)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(headDateElementKey).Name));
            stringBuilder.Append("Date Value");
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(headDateElementKey).Name));
            
            string headFileRegDateElementKey = $"{headElementKey}.{nameof(model.Head.FileRegDate)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(headFileRegDateElementKey).Name));
            stringBuilder.Append("FileRegDate Value");
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(headFileRegDateElementKey).Name));
            
            string headFileRegNumElementKey = $"{headElementKey}.{nameof(model.Head.FileRegNum)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(headFileRegNumElementKey).Name));
            stringBuilder.Append("FileRegNum Value");
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(headFileRegNumElementKey).Name));
            
            
            string headPrevFileElementKey = $"{headElementKey}.{nameof(model.Head.PrevFile)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(headPrevFileElementKey).Name));
            
            string headPrevFileFileRegDateElementKey = $"{headPrevFileElementKey}.{nameof(model.Head.PrevFile.FileRegDate)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(headPrevFileFileRegDateElementKey).Name));
            stringBuilder.Append("PrevFile.FileRegDate Value");
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(headPrevFileFileRegDateElementKey).Name));
            
            string headPrevFileFileRegNumElementKey = $"{headPrevFileElementKey}.{nameof(model.Head.PrevFile.FileRegNum)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(headPrevFileFileRegNumElementKey).Name));
            stringBuilder.Append("PrevFile.FileRegNum Value");
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(headPrevFileFileRegNumElementKey).Name));
            
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(headPrevFileElementKey).Name));

            #endregion
            
            #region Close Header Element

            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(headElementKey).Name));

            #endregion


            #region Info Part

            GetInfoParts(stringBuilder, model, modelInfoCollector);

            #endregion
            
            
            
            #region Open Footer Element

            string footerElementKey = $"{model.GetType().Name}.{nameof(model.Footer)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(footerElementKey).Name));

            #endregion
            
            #region FillFooter

            string footerSubjectsCountElementKey = $"{footerElementKey}.{nameof(model.Footer.SubjectsCount)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(footerSubjectsCountElementKey).Name));
            stringBuilder.Append("999");
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(footerSubjectsCountElementKey).Name));
            
            string footerRecordsCountElementKey = $"{footerElementKey}.{nameof(model.Footer.RecordsCount)}";
            stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(footerRecordsCountElementKey).Name));
            stringBuilder.Append("999");
            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(footerRecordsCountElementKey).Name));

            #endregion
            
            #region Close Footer Element

            stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(footerElementKey).Name));

            #endregion
            
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