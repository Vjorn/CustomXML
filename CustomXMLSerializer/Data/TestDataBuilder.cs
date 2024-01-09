using System.Text;
using CustomXMLSerializer.Core.Attributes.Helpers;
using CustomXMLSerializer.Core.Helpers;
using CustomXMLSerializer.Data.Builders;
using CustomXMLSerializer.Models;
using CustomXMLSerializer.Models.Parts.Events.Enums;

namespace CustomXMLSerializer.Data;

public class TestDataBuilder
{
    private int _recordIndex = 0;
    private int _recordIndexMax = 500;
    private int _templateLength = 0;
    private int _currentLength = 0;
    private int _maxLength = 5000;
    
    public void BuildData(SerializingTestModel model, ModelInfoCollector modelInfoCollector)
    {
        string filePath = $"text_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}_{Guid.NewGuid()}.xml";
        bool outOfLength = false;
        
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            string openRootString = GetRootString(model, modelInfoCollector);
            string closeRootString = XmlStringBuilder.WriteEndElement(modelInfoCollector.GetRootElementInfo().Name);
            string headerString = GetHeaderString(model, modelInfoCollector);
            
            _templateLength = Encoding.UTF8.GetBytes(openRootString).Length 
                             + Encoding.UTF8.GetBytes(closeRootString).Length
                             + Encoding.UTF8.GetBytes(headerString).Length
                             + Encoding.UTF8.GetBytes(GetFooterString(model, modelInfoCollector)).Length;

            _currentLength = _templateLength;
            
            stringBuilder.Append(openRootString); // Open Root Element
            stringBuilder.Append(headerString); // Append header
            
            RecordsBuilder recordsBuilder = new RecordsBuilder(_templateLength, _maxLength);

            for (int i = _recordIndex; i < _recordIndexMax; i++)
            {
                _recordIndex = i;
                string subjectMainChangedEventString =
                    recordsBuilder.GetRecordString(EventType.SubjectMainChanged, _recordIndex);

                int preCalculatedLength = _currentLength + Encoding.UTF8.GetBytes(subjectMainChangedEventString).Length;

                if (preCalculatedLength > _maxLength)
                {
                    outOfLength = true;
                    break;
                }

                _currentLength = preCalculatedLength;
                stringBuilder.Append(subjectMainChangedEventString);
            }
            
            stringBuilder.Append(GetFooterString(model, modelInfoCollector)); // Append footer
            stringBuilder.Append(closeRootString); // Close Root Element

            string result = stringBuilder.ToString();
            
            byte[] startRootElementBytes = Encoding.UTF8.GetBytes(result);
            
            fileStream.Write(startRootElementBytes, 0, startRootElementBytes.Length);
            fileStream.Flush();
        }
        
        if (outOfLength)
        {
            BuildData(model, modelInfoCollector);
        }
    }

    private string GetRootString(SerializingTestModel model, ModelInfoCollector modelInfoCollector)
    {
        StringBuilder rootStringBuilder = new StringBuilder();
        
        string versionAttributeKey = $"{model.GetType().Name}.{nameof(model.Version)}";
        rootStringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetRootElementInfo().Name, true));
        rootStringBuilder.Append(XmlStringBuilder.WriteAttributeString(modelInfoCollector.GetAttributeByKey(versionAttributeKey).Name,
            modelInfoCollector.GetAttributeByKey(versionAttributeKey).DefaultValue));
        rootStringBuilder.Append(">");
        
        return rootStringBuilder.ToString();
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

    private string GetFooterString(SerializingTestModel model, 
        ModelInfoCollector modelInfoCollector, int subjectsCount = 9999999, int recordsCount = 9999999)
    {
        StringBuilder footerStringBuilder = new StringBuilder();

        string footerElementKey = $"{model.GetType().Name}.{nameof(model.Footer)}";
        footerStringBuilder.AppendElementStart(modelInfoCollector.GetElementByKey(footerElementKey).Name);
        
        footerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{footerElementKey}.{nameof(model.Footer.SubjectsCount)}").Name,
            subjectsCount.ToString());
        
        footerStringBuilder.AppendElementWithValue(
            modelInfoCollector.GetElementByKey($"{footerElementKey}.{nameof(model.Footer.RecordsCount)}").Name,
            recordsCount.ToString());
        
        footerStringBuilder.AppendElementEnd(modelInfoCollector.GetElementByKey(footerElementKey).Name);
        
        return footerStringBuilder.ToString();
    }

    // private void GetInfoParts(StringBuilder stringBuilder, SerializingTestModel model, ModelInfoCollector modelInfoCollector)
    // {
    //     for (int i = 0; i < 5; i++)
    //     {
    //         string infoPartElementKey = $"{model.GetType().Name}.{nameof(model.InfoPart)}";
    //         stringBuilder.Append(XmlStringBuilder.WriteStartElement(modelInfoCollector.GetElementByKey(infoPartElementKey).Name, true));
    //         
    //         string infoPartRecordNumberAttributeKey = $"{infoPartElementKey}.{nameof(InfoPart.RecordNumber)}";
    //         stringBuilder.Append(XmlStringBuilder.WriteAttributeString(modelInfoCollector.GetAttributeByKey(infoPartRecordNumberAttributeKey).Name,
    //             $"Recnumber is {i}"));
    //         
    //         stringBuilder.Append(">");
    //         
    //         stringBuilder.Append(XmlStringBuilder.WriteEndElement(modelInfoCollector.GetElementByKey(infoPartElementKey).Name));
    //     }
    // }
}