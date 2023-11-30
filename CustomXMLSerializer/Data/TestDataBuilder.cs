using System.Reflection;
using System.Text;
using CustomXMLSerializer.Core;
using CustomXMLSerializer.Core.Attributes;
using CustomXMLSerializer.Core.Attributes.Helpers;
using CustomXMLSerializer.Core.Helpers;
using CustomXMLSerializer.Models;

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
            string rootAttribute1Key = $"{model.GetType().Name}.{nameof(model.RootAttribute1)}";
            string rootAttribute2Key = $"{model.GetType().Name}.{nameof(model.RootAttribute2)}";
            stringBuilder.Append($"<{modelInfoCollector.GetRootElementInfo().Name}");
            stringBuilder.Append(XmlStringBuilder.WriteAttributeString(modelInfoCollector.GetAttributeByKey(versionAttributeKey).Name,
                modelInfoCollector.GetAttributeByKey(versionAttributeKey).DefaultValue));
            stringBuilder.Append(XmlStringBuilder.WriteAttributeString(modelInfoCollector.GetAttributeByKey(rootAttribute1Key).Name,
                "RootAttribute1"));
            stringBuilder.Append(XmlStringBuilder.WriteAttributeString(modelInfoCollector.GetAttributeByKey(rootAttribute2Key).Name,
                "RootAttribute2"));
            stringBuilder.Append(">");

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

    private IEnumerable<Header> FillHeaderBlock()
    {
        for (int i = 0; i < 10; i++)
        {
            Header headerBlockElement = new Header();

            headerBlockElement.Header1 = $"Header1 - {i}";
            headerBlockElement.Header2 = $"Header2 - {i}";
            headerBlockElement.Header3 = $"Header3 - {i}";
            
            yield return headerBlockElement;
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    private IEnumerable<ElementAttribute>? GetElementAttributes<T>(T element) where T : class?
    {
        Type? type = element?.GetType();

        IEnumerable<PropertyInfo>? attributeProperties = type?.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomAttributeAttribute>());

        if (attributeProperties != null)
        {
            foreach (PropertyInfo attributeProperty in attributeProperties)
            {
                CustomXmlAttributeAttribute? xmlAttribute = attributeProperty
                    .GetCustomAttribute<CustomXmlAttributeAttribute>();

                object? attributeValue = attributeProperty.GetValue(element);

                if (xmlAttribute is null || attributeValue is null)
                {
                    continue;
                }

                ElementAttribute elementAttribute = new ElementAttribute();
                elementAttribute.AttributeName = xmlAttribute.ElementName;
                elementAttribute.AttributeValue = attributeValue.ToString();

                yield return elementAttribute;
            }
        }
    }

    private IEnumerable<InfoPart> FillInfoBlock()
    {
        for (int i = 0; i < 10; i++)
        {
            InfoPart infoPart = new InfoPart();

            infoPart.InfoPartElement1 = $"InfoPartElement1 - {i}";
            infoPart.InfoPartElement2 = $"InfoPartElement2 - {i}";
            infoPart.InfoPartElement3 = $"InfoPartElement3 - {i}";
            
            yield return infoPart;
        }
    }
}