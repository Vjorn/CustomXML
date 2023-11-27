using System.Reflection;

namespace CustomXMLSerializer.Core.Attributes.Helpers;

public static class AttributeHelper
{
    public static bool HasAttributeOfType<T>(this MemberInfo member) where T : Attribute
    {
        return member.GetCustomAttribute<T>() is not null;
    }
}