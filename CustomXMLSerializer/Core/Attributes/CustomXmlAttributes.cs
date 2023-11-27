using System.Diagnostics.CodeAnalysis;

namespace CustomXMLSerializer.Core.Attributes;



[AttributeUsage(AttributeTargets.Class)]
public class CustomXmlRootAttribute : CustomElementAttribute { }


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class CustomXmlHeaderElementAttribute : CustomElementAttribute { }


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class CustomXmlFooterElementAttribute : CustomElementAttribute { }


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class CustomXmlElementAttribute : CustomElementAttribute { }




[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class CustomXmlAttributeAttribute : CustomAttributeAttribute { }