using CustomXMLSerializer.Core;
using CustomXMLSerializer.Data;
using CustomXMLSerializer.Models;

Console.WriteLine("Hello, World!");

TestDataBuilder builder = new TestDataBuilder();
SerializingTestModel root = builder.BuildData();

OptimizedXmlSerializer serializer = new OptimizedXmlSerializer();
serializer.Serialize(root, "output.xml");


Console.WriteLine("Bye, World!");