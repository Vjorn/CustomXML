using System.Reflection;
using System.Xml;
using CustomXMLSerializer.Core.Attributes;
using CustomXMLSerializer.Core.Attributes.Helpers;

namespace CustomXMLSerializer.Core;

public class OptimizedXmlSerializer
{
    private readonly long _maxFileSize;
    private long _mainContentPosition;
    
    public OptimizedXmlSerializer(long maxFileSize)
    {
        this._maxFileSize = maxFileSize;
    }
    
    public async Task SerializeToFileAsync<T>(string filePath, T data) where T : class
    {
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "    ",
            Async = true
        };

        using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
        using (XmlWriter writer = XmlWriter.Create(stream, settings))
        {
            CustomXmlRootAttribute? xmlRootAttribute = (CustomXmlRootAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(CustomXmlRootAttribute))!;
            
            if (xmlRootAttribute == null)
            {
                throw new ArgumentNullException($"{nameof(CustomXmlRootAttribute)} doesn't set as class root attribute");
            }
            
            await writer.WriteStartElementAsync(null, xmlRootAttribute.ElementName, null);

            await SerializeAsync(stream, writer, data, data.GetType());
            
            await writer.WriteEndElementAsync(); // Close root element
        }
    }

    private async Task<bool> CheckFileSizeAsync(XmlWriter writer)
    {
        // Implement logic to check the file size
        // Example: return stream.Length > maxSize;
        return false;
    }

    private async Task SerializeAsync<T>(FileStream stream, XmlWriter writer, T data, Type parentType) where T : class
    {
        Type type = data.GetType();

        IEnumerable<PropertyInfo> attributeProperties = type.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomAttributeAttribute>());

        foreach (PropertyInfo attributeProperty in attributeProperties)
        {
            CustomXmlAttributeAttribute? xmlAttribute = attributeProperty
                .GetCustomAttribute<CustomXmlAttributeAttribute>();
            
            object? attributeValue = attributeProperty.GetValue(data);
            
            if (xmlAttribute is null || attributeValue is null)
            {
                continue;
            }
            
            await writer.WriteAttributeStringAsync(null, xmlAttribute.ElementName, null, attributeValue.ToString());
        }
        
        
        IEnumerable<PropertyInfo> headerElementProperties = type.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomXmlHeaderElementAttribute>());

        foreach (PropertyInfo headerElementProperty in headerElementProperties)
        {
            if (parentType == typeof(CustomXmlElementAttribute))
            {
                throw new ArgumentOutOfRangeException(@$"{nameof(type)} - {headerElementProperty.Name} : 
{nameof(CustomXmlHeaderElementAttribute)} elements couldn't be placed in {nameof(CustomXmlElementAttribute)} elements");
            }
            
            if (parentType == typeof(CustomXmlFooterElementAttribute))
            {
                throw new ArgumentOutOfRangeException(@$"{nameof(type)} - {headerElementProperty.Name} : 
{nameof(CustomXmlHeaderElementAttribute)} elements couldn't be placed in {nameof(CustomXmlFooterElementAttribute)} elements");
            }
            
            CustomXmlHeaderElementAttribute? customXmlElement = headerElementProperty.GetCustomAttribute<CustomXmlHeaderElementAttribute>();
            string customXmlElementName = customXmlElement is null 
                ? headerElementProperty.Name 
                : customXmlElement.ElementName;
            
            object? value = headerElementProperty.GetValue(data);
            
            if (value is not null) // TODO: Здесь можно сделать триггер на добавление элемента если даже он null
            {
                await writer.WriteStartElementAsync(null, customXmlElementName, null);
                
                if (!headerElementProperty.PropertyType.IsClass || headerElementProperty.PropertyType == typeof(string))
                {
                    await writer.WriteStringAsync(value.ToString());
                    await writer.WriteEndElementAsync(); 
                    _mainContentPosition = stream.Position;
                    continue;
                }
                    
                if (headerElementProperty.PropertyType == typeof(IEnumerable<>))
                {
                    IEnumerable<object> list = (IEnumerable<object>)value;
                    foreach (object item in list)
                    {
                        await SerializeAsync(stream, writer, item, type);
                    }
                }
                else
                {
                    await SerializeAsync(stream, writer, value, type);
                }
            }

            await writer.WriteEndElementAsync(); // Close element
            _mainContentPosition = stream.Position;
        }
        
        IEnumerable<PropertyInfo> generalElementProperties = type.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomXmlElementAttribute>());
        
        foreach (PropertyInfo generalElementProperty in generalElementProperties)
        {
            CustomXmlElementAttribute? customXmlElement = generalElementProperty.GetCustomAttribute<CustomXmlElementAttribute>();
            string customXmlElementName = customXmlElement is null 
                ? generalElementProperty.Name 
                : customXmlElement.ElementName;
            
            object? value = generalElementProperty.GetValue(data);
            
            if (value is not null) // Здесь можно сделать триггер на добавление элемента если даже он null
            {
                await writer.WriteStartElementAsync(null, customXmlElementName, null);
                
                if (!generalElementProperty.PropertyType.IsClass || generalElementProperty.PropertyType == typeof(string))
                {
                    await writer.WriteStringAsync(value.ToString());
                    await writer.WriteEndElementAsync(); 
                    continue;
                }
                    
                if (generalElementProperty.PropertyType == typeof(IEnumerable<>))
                {
                    IEnumerable<object> list = (IEnumerable<object>)value;
                    foreach (object item in list)
                    {
                        await SerializeAsync(stream, writer, item, type);
                    }
                }
                else
                {
                    await SerializeAsync(stream, writer, value, type);
                }
            }

            await writer.WriteEndElementAsync(); // Close element
        }
        
        
        
        IEnumerable<PropertyInfo> footerElementProperties = type.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomXmlFooterElementAttribute>());
        
        foreach (PropertyInfo footerElementProperty in footerElementProperties)
        {
            CustomXmlFooterElementAttribute? customXmlElement = footerElementProperty.GetCustomAttribute<CustomXmlFooterElementAttribute>();
            string customXmlElementName = customXmlElement is null 
                ? footerElementProperty.Name 
                : customXmlElement.ElementName;
            
            object? value = footerElementProperty.GetValue(data);
            
            if (value is not null)
            {
                await writer.WriteStartElementAsync(null, customXmlElementName, null);
                
                if (!footerElementProperty.PropertyType.IsClass || footerElementProperty.PropertyType == typeof(string))
                {
                    await writer.WriteStringAsync(value.ToString());
                    await writer.WriteEndElementAsync(); 
                    continue;
                }
                    
                if (footerElementProperty.PropertyType == typeof(IEnumerable<>))
                {
                    IEnumerable<object> list = (IEnumerable<object>)value;
                    foreach (object item in list)
                    {
                        await SerializeAsync(stream, writer, item, type);
                    }
                }
                else
                {
                    await SerializeAsync(stream, writer, value, type);
                }
            }

            await writer.WriteEndElementAsync(); // Close element
        }
    }
    
}