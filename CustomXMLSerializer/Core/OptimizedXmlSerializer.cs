using System.Reflection;
using System.Text;
using CustomXMLSerializer.Core.Attributes;
using CustomXMLSerializer.Core.Attributes.Helpers;
using CustomXMLSerializer.Models;

namespace CustomXMLSerializer.Core;

public class OptimizedXmlSerializer
{
    private const int MaxFileSize = 1024; // Максимальный размер файла

    public void Serialize(SerializingTestModel root, string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            XmlStringBuilder xmlStringBuilder = new XmlStringBuilder();
            
            CustomXmlRootAttribute? xmlRootAttribute = (CustomXmlRootAttribute)Attribute.GetCustomAttribute(
                typeof(SerializingTestModel), typeof(CustomXmlRootAttribute))!;
            
            if (xmlRootAttribute == null)
            {
                throw new ArgumentNullException($"{nameof(CustomXmlRootAttribute)} doesn't set as class root attribute");
            }
            
            byte[] startRootElementBytes = Encoding.UTF8.GetBytes(xmlStringBuilder
                .WriteStartElement(xmlRootAttribute.ElementName, GetElementAttributes(root)));
            fileStream.Write(startRootElementBytes, 0, startRootElementBytes.Length);
            fileStream.Flush();

            string headerString = "";
            byte[] headerElementsBytes = Encoding.UTF8.GetBytes(GetHeaderElements(headerString, xmlStringBuilder, root, typeof(SerializingTestModel)));
            fileStream.Write(headerElementsBytes, 0, headerElementsBytes.Length);
            
            byte[] endRootElementBytes = Encoding.UTF8.GetBytes(xmlStringBuilder
                .WriteEndElement(xmlRootAttribute.ElementName));
            fileStream.Write(endRootElementBytes, 0, endRootElementBytes.Length);
            
            fileStream.Seek(0, SeekOrigin.Begin);
        }
        
        // using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        // {
        //     
        //     
        //     
        //     
        //     using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
        //     {
        //         XmlStreamWriter xmlWriter = new XmlStreamWriter(writer);
        //         
        //         
        //         
        //         
        //         
        //         CustomXmlRootAttribute? xmlRootAttribute = (CustomXmlRootAttribute)Attribute.GetCustomAttribute(
        //             typeof(SerializingTestModel), typeof(CustomXmlRootAttribute))!;
        //     
        //         if (xmlRootAttribute == null)
        //         {
        //             throw new ArgumentNullException($"{nameof(CustomXmlRootAttribute)} doesn't set as class root attribute");
        //         }
        //         
        //         xmlWriter.WriteStartElement(xmlRootAttribute.ElementName, GetElementAttributes(root));
        //         
        //         
        //         
        //         
        //         
        //         // Сохраняем начальное положение стрима
        //         long startOffset = fileStream.Position;
        //
        //         // Сериализуем блок Head
        //         xmlWriter.WriteStartElement("head");
        //         xmlWriter.WriteEndElement();
        //
        //         // Записываем текущее положение стрима после блока Head
        //         long headEndOffset = fileStream.Position;
        //
        //         // Сериализуем блок InfoBlock
        //         xmlWriter.WriteStartElement("info");
        //
        //         foreach (InfoPart infoPart in root.InfoBlock)
        //         {
        //             xmlWriter.WriteStartElement("infoElement");
        //             xmlWriter.WriteEndElement();
        //         }
        //
        //         xmlWriter.WriteEndElement(); // Закрываем блок InfoBlock
        //
        //         // Считаем размер файла
        //         long fileSize = fileStream.Length;
        //
        //         // Проверяем размер файла после добавления InfoBlock
        //         if (fileSize > MaxFileSize)
        //         {
        //             // Возвращаемся к началу блока InfoBlock
        //             fileStream.Position = headEndOffset;
        //
        //             // Создаем новый файл с продолжением предыдущего, но с таким же Head
        //             xmlWriter.WriteEndElement(); // Закрываем блок root
        //             xmlWriter.Flush();
        //
        //             // Выводим текущий буфер в файл
        //             writer.Flush();
        //
        //             // Создаем новый файл с продолжением предыдущего, но с таким же Head
        //             fileStream.Position = startOffset;
        //             xmlWriter.WriteStartElement("root");
        //             xmlWriter.WriteAttributeString("version", root.Version);
        //             xmlWriter.WriteStartElement("head");
        //             xmlWriter.WriteEndElement();
        //         }
        //
        //         // Сериализуем блок Footer
        //         xmlWriter.WriteStartElement("footer");
        //         xmlWriter.WriteEndElement();
        //
        //         xmlWriter.WriteEndElement(xmlRootAttribute.ElementName); // Закрываем блок root
        //     }
        // }
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

    private string GetHeaderElements<T>(string headerString, XmlStringBuilder xmlStringBuilder, T element, Type parentType) where T : class
    {
        Type type = element.GetType();

        IEnumerable<PropertyInfo> headerProperties = type.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomXmlHeaderElementAttribute>());

        foreach (PropertyInfo headerProperty in headerProperties)
        {
            if (parentType == typeof(CustomXmlElementAttribute))
            {
                throw new ArgumentOutOfRangeException(@$"{nameof(type)} - {headerProperty.Name} : 
{nameof(CustomXmlHeaderElementAttribute)} elements couldn't be placed in {nameof(CustomXmlElementAttribute)} elements");
            }
            if (parentType == typeof(CustomXmlFooterElementAttribute))
            {
                throw new ArgumentOutOfRangeException(@$"{nameof(type)} - {headerProperty.Name} : 
{nameof(CustomXmlHeaderElementAttribute)} elements couldn't be placed in {nameof(CustomXmlFooterElementAttribute)} elements");
            }
            
            CustomXmlHeaderElementAttribute? customXmlElement = headerProperty.GetCustomAttribute<CustomXmlHeaderElementAttribute>();
            string customXmlElementName = customXmlElement is null
                ? headerProperty.Name 
                : customXmlElement.ElementName;
            
            object? value = headerProperty.GetValue(element);

            IEnumerable<ElementAttribute>? elementAttributes = GetElementAttributes(value);
            
            if (value is not null) // TODO: Здесь можно сделать триггер на добавление элемента если даже он null
            {
                if (!headerProperty.PropertyType.IsClass || headerProperty.PropertyType == typeof(string))
                {
                    headerString += xmlStringBuilder.WriteStartElement(customXmlElementName, elementAttributes);
                    headerString += value.ToString();
                    headerString += xmlStringBuilder.WriteEndElement(customXmlElementName);
                    continue;
                }
                    
                if (headerProperty.PropertyType == typeof(IEnumerable<>))
                {
                    headerString += xmlStringBuilder.WriteStartElement(customXmlElementName, elementAttributes);
                    IEnumerable<object> list = (IEnumerable<object>)value;
                    foreach (object item in list)
                    {
                        headerString += GetHeaderElements(headerString, xmlStringBuilder, item, type);
                    }
                    headerString += xmlStringBuilder.WriteEndElement(customXmlElementName);
                    
                }
                else
                {
                    headerString += xmlStringBuilder.WriteStartElement(customXmlElementName, elementAttributes);
                    headerString = GetHeaderElements(headerString, xmlStringBuilder, value, type);
                    headerString += xmlStringBuilder.WriteEndElement(customXmlElementName);
                }
            }
        }
        
        IEnumerable<PropertyInfo> generalElementProperties = type.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomXmlElementAttribute>());

        foreach (PropertyInfo generalElementProperty in generalElementProperties)
        {
            CustomXmlElementAttribute? customXmlElement =
                generalElementProperty.GetCustomAttribute<CustomXmlElementAttribute>();
            string customXmlElementName = customXmlElement is null
                ? generalElementProperty.Name
                : customXmlElement.ElementName;

            object? value = generalElementProperty.GetValue(element);
            
            IEnumerable<ElementAttribute>? elementAttributes = GetElementAttributes(value);

            if (value is not null) // Здесь можно сделать триггер на добавление элемента если даже он null
            {
                if (!generalElementProperty.PropertyType.IsClass || generalElementProperty.PropertyType == typeof(string))
                {
                    headerString += xmlStringBuilder.WriteStartElement(customXmlElementName, null);
                    headerString += value.ToString();
                    headerString += xmlStringBuilder.WriteEndElement(customXmlElementName);
                    continue;
                }
                    
                if (generalElementProperty.PropertyType == typeof(IEnumerable<>))
                {
                    headerString = xmlStringBuilder.WriteStartElement(customXmlElementName, elementAttributes);
                    IEnumerable<object> list = (IEnumerable<object>)value;
                    foreach (object item in list)
                    {
                        headerString += GetHeaderElements(headerString, xmlStringBuilder, item, type);
                    }
                    headerString += xmlStringBuilder.WriteEndElement(customXmlElementName);
                }
                else
                {
                    headerString += xmlStringBuilder.WriteStartElement(customXmlElementName, elementAttributes);
                    headerString += GetHeaderElements(headerString, xmlStringBuilder, value, type);
                    headerString += xmlStringBuilder.WriteEndElement(customXmlElementName);
                }
            }
        }

        return headerString;
    }
}