using System.Reflection;
using CustomXMLSerializer.Models;

namespace CustomXMLSerializer.Core.Attributes.Helpers;

public class ModelInfoCollector
{
    private readonly List<ElementInfo> _modelInformation = new List<ElementInfo>();
    
    public ModelInfoCollector(Type modelType)
    {
        CollectModelInformation(modelType);
    }

    public IEnumerable<ElementInfo> GetModelInformation()
    {
        return _modelInformation;
    }

    public ElementInfo GetRootElementInfo()
    {
        return _modelInformation.First(e => e.Type == ElementType.Root);
    }

    public ElementInfo GetAttributeByKey(string attributeKey)
    {
        return _modelInformation.First(e => e.Key == attributeKey && e.Type == ElementType.Attribute);
    }
    
    public ElementInfo GetElementByKey(string attributeKey)
    {
        return _modelInformation.First(e => e.Key == attributeKey && e.Type == ElementType.Element);
    }
    
    public string? GetAttributeNameByKey(string attributeKey)
    {
        return _modelInformation
            .Where(e => e.Key == attributeKey && e.Type == ElementType.Attribute)
            .Select(e => e.Name)
            .First();
    }
    
    private void CollectModelInformation(Type modelType)
    {
        CustomXmlRootAttribute? xmlRootAttribute = (CustomXmlRootAttribute)Attribute.GetCustomAttribute(
            modelType, typeof(CustomXmlRootAttribute))!;
            
        if (xmlRootAttribute == null)
        {
            throw new ArgumentNullException($"{nameof(CustomXmlRootAttribute)} doesn't set as class root attribute");
        }

        _modelInformation.Add(new ElementInfo(modelType.Name, xmlRootAttribute.ElementName, ElementType.Root));
        
        foreach (ElementInfo commonElement in GetCommonElements(modelType.Name, modelType))
        {
            _modelInformation.Add(commonElement);
        }
    }

    private IEnumerable<ElementInfo> GetAttributes(string propertyKeyRoot, Type type)
    {
        IEnumerable<PropertyInfo> attributeProperties = type.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomAttributeAttribute>());

        foreach (PropertyInfo attributeProperty in attributeProperties)
        {
            CustomXmlAttributeAttribute? xmlAttribute = attributeProperty
                .GetCustomAttribute<CustomXmlAttributeAttribute>();
            string? customXmlAttributeName = xmlAttribute is null
                ? attributeProperty.Name
                : xmlAttribute.ElementName;

            if (attributeProperty.PropertyType != typeof(string))
            {
                throw new Exception($"{nameof(CustomXmlAttributeAttribute)} at {propertyKeyRoot}.{attributeProperty.Name} " +
                                    $"must be a string type element"); // TODO Specify right ExceptionType
            }

            if (xmlAttribute is null)
            {
                continue;
            }

            yield return new ElementInfo($"{propertyKeyRoot}.{attributeProperty.Name}", customXmlAttributeName, ElementType.Attribute, xmlAttribute.DefaultValue);
        }
    }

    private IEnumerable<ElementInfo> GetCommonElements(string propertyKeyRoot, Type headerType)
    {
        foreach (ElementInfo attributeInfo in GetAttributes(propertyKeyRoot, headerType))
        {
            yield return attributeInfo;
        }
        
        IEnumerable<PropertyInfo> elementProperties = headerType.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomXmlElementAttribute>());

        foreach (PropertyInfo elementProperty in elementProperties)
        {
            string elementPropertyKey = $"{propertyKeyRoot}.{elementProperty.Name}";
            
            CustomXmlElementAttribute? customXmlElement =
                elementProperty.GetCustomAttribute<CustomXmlElementAttribute>();
            
            if (customXmlElement is null)
            {
                continue;
            }
            
            string? customXmlElementName = customXmlElement.ElementName ?? elementProperty.Name;
            
            if (elementProperty.PropertyType.IsGenericType && 
                elementProperty.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                Type[] genericArguments = elementProperty.PropertyType.GetGenericArguments();
                
                foreach (Type argument in genericArguments)
                {
                    
                    foreach (ElementInfo subElement in GetCommonElements(elementPropertyKey, argument))
                    {
                        yield return subElement;
                    }
                    
                    yield return new ElementInfo(elementPropertyKey, customXmlElementName, ElementType.Element);
                }
                
                continue;
            }
            
            if (!elementProperty.PropertyType.IsClass || elementProperty.PropertyType == typeof(string))
            {
                yield return new ElementInfo(elementPropertyKey, customXmlElementName, ElementType.Element);
            }
            else
            {
                foreach (ElementInfo subElement in GetCommonElements(elementPropertyKey, elementProperty.PropertyType))
                {
                    yield return subElement;
                }
                
                yield return new ElementInfo(elementPropertyKey, customXmlElementName, ElementType.Element);
            }
        }
    }
}