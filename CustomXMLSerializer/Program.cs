using CustomXMLSerializer.Core;
using CustomXMLSerializer.Core.Attributes.Helpers;
using CustomXMLSerializer.Data;
using CustomXMLSerializer.Models;

Console.WriteLine("Hello, World!");

Dictionary<string, ElementInfo> modelInfo = ModelInfoCollecter.CollectModelInformation(typeof(SerializingTestModel));



// TestDataBuilder builder = new TestDataBuilder();
// SerializingTestModel root = builder.BuildData();
//
// OptimizedXmlSerializer serializer = new OptimizedXmlSerializer();
// serializer.Serialize(root, "output.xml");


Console.WriteLine("Bye, World!");