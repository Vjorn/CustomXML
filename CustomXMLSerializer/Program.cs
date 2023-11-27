// See https://aka.ms/new-console-template for more information

using CustomXMLSerializer.Core;
using CustomXMLSerializer.Models;

Console.WriteLine("Hello, World!");

string filePath = "test.xml";


SerializingTestModel testModel = new SerializingTestModel();
testModel.Attribute1 = "Attribute1";
testModel.Attribute2 = "Attribute2";
// testModel.Attribute3 = "Attribute3"; // skip Attribute3 for test
testModel.Attribute4 = "Attribute4";
testModel.Attribute5 = "Attribute5";
testModel.Header1 = new Header1();
testModel.Header1.Header1_StringElement1 = "Header1_StringElement1";
testModel.Header2 = new Header2();
testModel.Header2.Header2_StringElement1 = "Header2_StringElement1";
testModel.Header3 = new Header3();
testModel.Header3.Header3_StringElement1 = "Header3_StringElement1";
testModel.Footer1 = new Footer1();
testModel.Footer1.Footer1_StringElement1 = "Footer1_StringElement1";
testModel.Footer2 = new Footer2();
testModel.Footer2.Footer2_StringElement1 = "Footer2_StringElement1";
testModel.Footer3 = new Footer3();
testModel.Footer3.Footer3_StringElement1 = "Footer3_StringElement1";

OptimizedXmlSerializer customSerializer = new OptimizedXmlSerializer(100000);
await customSerializer.SerializeToFileAsync(filePath, testModel);

Console.WriteLine("Bye, World!");