using System.Reflection;
using System.Text;
using CustomXMLSerializer.Core;
using CustomXMLSerializer.Core.Attributes;
using CustomXMLSerializer.Core.Attributes.Helpers;
using CustomXMLSerializer.Models;

namespace CustomXMLSerializer.Data;

public class TestDataBuilder
{
    public SerializingTestModel BuildData()
    {
        string filePath = "text.xml";
        SerializingTestModel model = new SerializingTestModel();
        
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            XmlStringBuilder xmlStringBuilder = new XmlStringBuilder();
            
            CustomXmlRootAttribute? xmlRootAttribute = (CustomXmlRootAttribute)Attribute.GetCustomAttribute(
                typeof(SerializingTestModel), typeof(CustomXmlRootAttribute))!;
            
            if (xmlRootAttribute == null)
            {
                throw new ArgumentNullException($"{nameof(CustomXmlRootAttribute)} doesn't set as class root attribute");
            }
            
            model.RootAttribute1 = "RootAttribute1";
            byte[] startRootElementBytes = Encoding.UTF8.GetBytes(xmlStringBuilder
                .WriteStartElement(xmlRootAttribute.ElementName, GetElementAttributes(model)));
            fileStream.Write(startRootElementBytes, 0, startRootElementBytes.Length);
            fileStream.Flush();
            
            

            model.HeaderClass = new Header();
            model.HeaderClass.Header1 = "Header2.Header1";
            model.HeaderClass.Header2 = "Header2.Header2";
            model.HeaderClass.Header3 = "Header2.Header3";

            model.HeaderBlock = FillHeaderBlock();
            
            byte[] endRootElementBytes = Encoding.UTF8.GetBytes(xmlStringBuilder
                .WriteEndElement(xmlRootAttribute.ElementName));
            fileStream.Write(endRootElementBytes, 0, endRootElementBytes.Length);
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