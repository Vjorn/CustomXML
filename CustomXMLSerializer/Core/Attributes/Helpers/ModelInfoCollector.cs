using System.Reflection;
using CustomXMLSerializer.Models;

namespace CustomXMLSerializer.Core.Attributes.Helpers;

public class ModelInfoCollector
{
    private readonly IEnumerable<ElementInfo> _modelInformation;
    
    public ModelInfoCollector(Type modelType)
    {
        _modelInformation = CollectModelInformation(modelType);
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
    
    public string? GetAttributeNameByKey(string attributeKey)
    {
        return _modelInformation
            .Where(e => e.Key == attributeKey && e.Type == ElementType.Attribute)
            .Select(e => e.Name)
            .First();
    }
    
    private IEnumerable<ElementInfo> CollectModelInformation(Type modelType)
    {
        CustomXmlRootAttribute? xmlRootAttribute = (CustomXmlRootAttribute)Attribute.GetCustomAttribute(
            modelType, typeof(CustomXmlRootAttribute))!;
            
        if (xmlRootAttribute == null)
        {
            throw new ArgumentNullException($"{nameof(CustomXmlRootAttribute)} doesn't set as class root attribute");
        }

        yield return new ElementInfo(modelType.Name, xmlRootAttribute.ElementName, ElementType.Root);
        
        foreach (ElementInfo headerElement in GetHeaderElements(modelType))
        {
            yield return headerElement;
        }
        
        foreach (ElementInfo commonElement in GetCommonElements(modelType.Name, modelType))
        {
            yield return commonElement;
        }
    }

    private IEnumerable<ElementInfo> GetAttributes(string propertyKeyRoot, Type type, ElementType attributeType)
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

            yield return new ElementInfo($"{propertyKeyRoot}.{attributeProperty.Name}", customXmlAttributeName, attributeType, xmlAttribute.DefaultValue);
        }
    }

    private IEnumerable<ElementInfo> GetHeaderElements(Type rootType)
    {
        IEnumerable<PropertyInfo> headerProperties = rootType.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomXmlHeaderElementAttribute>());

        foreach (PropertyInfo headerProperty in headerProperties)
        {
            CustomXmlHeaderElementAttribute? xmlAttribute = headerProperty
                .GetCustomAttribute<CustomXmlHeaderElementAttribute>();

            if (xmlAttribute is null)
            {
                continue;
            }
            
            string headerPropertyKey = $"{rootType.Name}.{headerProperty.Name}";

            if (headerProperty.PropertyType.IsGenericType && 
                headerProperty.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                Type[] genericArguments = headerProperty.PropertyType.GetGenericArguments();
                
                foreach (Type argument in genericArguments)
                {
                    string blockPropertyKey = $"{headerPropertyKey}.{argument.Name}";
                    
                    foreach (ElementInfo subElement in GetCommonElements(blockPropertyKey, argument))
                    {
                        yield return subElement;
                    }
                    
                    yield return new ElementInfo(blockPropertyKey, xmlAttribute.ElementName, ElementType.HeaderBlock);
                }
                
                continue;
            }
            
            if (!headerProperty.PropertyType.IsClass || headerProperty.PropertyType == typeof(string))
            {
                yield return new ElementInfo(headerPropertyKey, xmlAttribute.ElementName, ElementType.HeaderElement);
            }

            if (headerProperty.PropertyType.IsClass)
            {
                foreach (ElementInfo subElement in GetCommonElements(headerPropertyKey, headerProperty.PropertyType))
                {
                    yield return subElement;
                }
            }
        }
    }

    private IEnumerable<ElementInfo> GetCommonElements(string headerPropertyKey, Type headerType)
    {
        foreach (ElementInfo attributeInfo in GetAttributes(headerPropertyKey, headerType, ElementType.Attribute))
        {
            yield return attributeInfo;
        }
        
        IEnumerable<PropertyInfo> subElementProperties = headerType.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomXmlElementAttribute>());

        foreach (PropertyInfo subElementProperty in subElementProperties)
        {
            string headerSubElementPropertyKey = $"{headerPropertyKey}.{subElementProperty.Name}";
            
            CustomXmlElementAttribute? customXmlElement =
                subElementProperty.GetCustomAttribute<CustomXmlElementAttribute>();
            string? customXmlElementName = customXmlElement is null
                ? subElementProperty.Name
                : customXmlElement.ElementName;
            
            if (subElementProperty.PropertyType.IsGenericType && 
                subElementProperty.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                Type[] genericArguments = subElementProperty.PropertyType.GetGenericArguments();
                
                foreach (Type argument in genericArguments)
                {
                    string blockPropertyKey = $"{headerSubElementPropertyKey}.{argument.Name}";
                    
                    foreach (ElementInfo subElement in GetCommonElements(blockPropertyKey, argument))
                    {
                        yield return subElement;
                    }
                    
                    yield return new ElementInfo(blockPropertyKey, customXmlElementName, ElementType.HeaderBlock);
                }
                
                continue;
            }
            
            if (!subElementProperty.PropertyType.IsClass || subElementProperty.PropertyType == typeof(string))
            {
                yield return new ElementInfo(headerSubElementPropertyKey, customXmlElementName, ElementType.HeaderElement);
            }

            if (subElementProperty.PropertyType.IsClass)
            {
                foreach (ElementInfo subElement in GetCommonElements(headerSubElementPropertyKey, subElementProperty.PropertyType))
                {
                    yield return subElement;
                }
            }
        }
    }
}