using CustomXMLSerializer.Core.Attributes.Helpers;
using CustomXMLSerializer.Data;
using CustomXMLSerializer.Models;

Console.WriteLine("Hello, World!");

TestDataBuilder builder = new TestDataBuilder();

        
SerializingTestModel model = new SerializingTestModel();
ModelInfoCollector modelInfoCollector = new ModelInfoCollector(model.GetType());

builder.BuildData(model, modelInfoCollector);

Console.WriteLine("Bye, World!");