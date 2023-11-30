using System.Reflection;
using CustomXMLSerializer.Models;

namespace CustomXMLSerializer.Core.Attributes.Helpers;

public static class ModelInfoCollecter
{
    public static Dictionary<string, ElementInfo> CollectModelInformation(Type modelType)
    {
        Dictionary<string, ElementInfo> modelInfoResult = new Dictionary<string, ElementInfo>();
        
        CustomXmlRootAttribute? xmlRootAttribute = (CustomXmlRootAttribute)Attribute.GetCustomAttribute(
            modelType, typeof(CustomXmlRootAttribute))!;
            
        if (xmlRootAttribute == null)
        {
            throw new ArgumentNullException($"{nameof(CustomXmlRootAttribute)} doesn't set as class root attribute");
        }
        
        modelInfoResult.Add(modelType.Name, new ElementInfo(xmlRootAttribute.ElementName, ElementType.Root));
        
        // foreach ((string Key, ElementInfo Value) rootAttribute in GetAttributes(modelType.Name, modelType, ElementType.RootAttribute))
        // {
        //     modelInfoResult.Add(rootAttribute.Key, rootAttribute.Value);
        // }
        
        foreach ((string Key, ElementInfo Value) headerElement in GetHeaderElements(modelType))
        {
            modelInfoResult.Add(headerElement.Key, headerElement.Value);
        }
        
        foreach ((string Key, ElementInfo Value) commonElement in GetCommonElements(modelType.Name, modelType))
        {
            modelInfoResult.Add(commonElement.Key, commonElement.Value);
        }
        
        return modelInfoResult;
    }

    private static IEnumerable<(string, ElementInfo)> GetAttributes(string propertyKeyRoot, Type type, ElementType attributeType)
    {
        IEnumerable<PropertyInfo> attributeProperties = type.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomAttributeAttribute>());

        foreach (PropertyInfo attributeProperty in attributeProperties)
        {
            CustomXmlAttributeAttribute? xmlAttribute = attributeProperty
                .GetCustomAttribute<CustomXmlAttributeAttribute>();
            string customXmlAttributeName = xmlAttribute is null
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

            yield return ($"{propertyKeyRoot}.{attributeProperty.Name}", 
                new ElementInfo(customXmlAttributeName, attributeType));
        }
    }

    private static IEnumerable<(string, ElementInfo)> GetHeaderElements(Type rootType)
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
                    
                    foreach ((string Key, ElementInfo Value) subElement in GetCommonElements(blockPropertyKey, argument))
                    {
                        yield return (subElement.Key, subElement.Value);
                    }
                    
                    yield return (blockPropertyKey, new ElementInfo(xmlAttribute.ElementName, ElementType.HeaderBlock));
                }
                
                continue;
            }
            
            if (!headerProperty.PropertyType.IsClass || headerProperty.PropertyType == typeof(string))
            {
                yield return (headerPropertyKey, new ElementInfo(xmlAttribute.ElementName, ElementType.HeaderElement));
            }

            if (headerProperty.PropertyType.IsClass)
            {
                foreach ((string Key, ElementInfo Value) subElement in GetCommonElements(headerPropertyKey, headerProperty.PropertyType))
                {
                    yield return (subElement.Key, subElement.Value);
                }
            }
        }
    }

    private static IEnumerable<(string, ElementInfo)> GetCommonElements(string headerPropertyKey, Type headerType)
    {
        foreach ((string Key, ElementInfo Value) attributeInfo in GetAttributes(headerPropertyKey, headerType, ElementType.Attribute))
        {
            yield return (attributeInfo.Key, attributeInfo.Value);
        }
        
        IEnumerable<PropertyInfo> subElementProperties = headerType.GetProperties()
            .Where(p => p.HasAttributeOfType<CustomXmlElementAttribute>());

        foreach (PropertyInfo subElementProperty in subElementProperties)
        {
            string headerSubElementPropertyKey = $"{headerPropertyKey}.{subElementProperty.Name}";
            
            CustomXmlElementAttribute? customXmlElement =
                subElementProperty.GetCustomAttribute<CustomXmlElementAttribute>();
            string customXmlElementName = customXmlElement is null
                ? subElementProperty.Name
                : customXmlElement.ElementName;
            
            if (subElementProperty.PropertyType.IsGenericType && 
                subElementProperty.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                Type[] genericArguments = subElementProperty.PropertyType.GetGenericArguments();
                
                foreach (Type argument in genericArguments)
                {
                    string blockPropertyKey = $"{headerSubElementPropertyKey}.{argument.Name}";
                    
                    foreach ((string Key, ElementInfo Value) subElement in GetCommonElements(blockPropertyKey, argument))
                    {
                        yield return (subElement.Key, subElement.Value);
                    }
                    
                    yield return (blockPropertyKey, new ElementInfo(customXmlElementName, ElementType.HeaderBlock));
                }
                
                continue;
            }
            
            if (!subElementProperty.PropertyType.IsClass || subElementProperty.PropertyType == typeof(string))
            {
                yield return (headerSubElementPropertyKey, new ElementInfo(customXmlElementName, ElementType.HeaderElement));
            }

            if (subElementProperty.PropertyType.IsClass)
            {
                foreach ((string Key, ElementInfo Value) subElement in GetCommonElements(headerSubElementPropertyKey, subElementProperty.PropertyType))
                {
                    yield return (subElement.Key, subElement.Value);
                }
            }
        }
    }
}