using CustomXMLSerializer.Data;
using CustomXMLSerializer.Models;

Console.WriteLine("Hello, World!");

TestDataBuilder builder = new TestDataBuilder();
SerializingTestModel root = builder.BuildData();

Console.WriteLine("Bye, World!");