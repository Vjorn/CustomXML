using CustomXMLSerializer.Models;

namespace CustomXMLSerializer.Data;

public class TestDataBuilder
{
    public SerializingTestModel BuildData()
    {
        SerializingTestModel model = new SerializingTestModel();
        model.RootAttribute1 = "RootAttribute1";
        model.HeaderString = "Header1";

        model.HeaderClass = new Header();
        model.HeaderClass.Header1 = "Header2.Header1";
        model.HeaderClass.Header2 = "Header2.Header2";
        model.HeaderClass.Header3 = "Header2.Header3";

        model.HeaderBlock = FillHeaderBlock();
        
        return model;
    }

    private IEnumerable<Header> FillHeaderBlock()
    {
        for (int i = 0; i < 10; i++)
        {
            Header headerBlockElement = new Header();

            headerBlockElement.Header1 = $"Header1 - {i}";
            headerBlockElement.Header2 = $"Header2 - {i}";
            headerBlockElement.Header3 = $"Header3 - {i}";
            
            yield return headerBlockElement;
        }
    }

    private IEnumerable<InfoPart> FillInfoBlock()
    {
        for (int i = 0; i < 10; i++)
        {
            InfoPart infoPart = new InfoPart();

            infoPart.InfoPartElement1 = $"InfoPartElement1 - {i}";
            infoPart.InfoPartElement2 = $"InfoPartElement2 - {i}";
            infoPart.InfoPartElement3 = $"InfoPartElement3 - {i}";
            
            yield return infoPart;
        }
    }
}